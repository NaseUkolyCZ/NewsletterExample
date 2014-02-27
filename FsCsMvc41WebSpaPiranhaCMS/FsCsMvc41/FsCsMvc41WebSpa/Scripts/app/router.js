(function (util) {    
    angular.module("recipientsApp.service", [], function ($provide) {
        $provide.factory("RecipientsService", ["$http", "$location", function($http, $location) {
            var recipientService = {};
            var recipients = [];

            recipientService.getAll = function(callback) {
                if (recipients.length === 0) {
                    $http.get("/api/recipients").success(function(data) {
                        recipients = data;
                        callback(recipients);
                    });
                } else {
                    callback(recipients);
                }
            };
            recipientService.deleteItem = function (id) {
                $http({
                    url: "/api/recipients",
                    method: "DELETE",
                    data: JSON.stringify(id),
                })
                .success(function () {
                    toastr.success("You have successfully deleted the recipient!", "Success!");
                    $location.path("/");
                })
                .error(function () {
                    toastr.error("There was an error deleting your new recipient", "<sad face>");
                });
            }

            recipientService.addItem = function (item) {
                recipients.push(item);
                $http({
                    url: "/api/recipients",
                    method: "POST",
                    data: JSON.stringify(item),
                })
                .success(function () {
                    toastr.success("You have successfully created a new recipient!", "Success!");
                    $location.path("/");
                })
                .error(function () {
                    recipients.pop();
                    toastr.error("There was an error creating your new recipient", "<sad face>");
                });
            };

            return recipientService;
        }]);
    });
    
    angular.module("recipientsapp", ["recipientsApp.service"])
        .config(["$routeProvider", function ($routeProvider) {
            $routeProvider
                .when("/create", { templateUrl: util.buildTemplateUrl("recipientCreate.htm") })
                .when("/delete/:smtpAddress", { templateUrl: util.buildTemplateUrl("recipientDelete.htm") })
                .otherwise({ redirectTo: "/", templateUrl: util.buildTemplateUrl("recipientDetail.htm") });
        }]);
})(appFsMvc.utility);

