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
                .otherwise({ redirectTo: "/", templateUrl: util.buildTemplateUrl("recipientDetail.htm") });
        }]);
})(appFsMvc.utility);

