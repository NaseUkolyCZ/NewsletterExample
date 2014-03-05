(function (module) {
    module.recipientDetailController = function ($scope, $routeParams, RecipientsService) {
        /*if (typeof ($routeParams) == "undefined") {
            RecipientsService.getAll(0, function (data) {
                $scope.recipients = data;
            });
        } else*/ {
            RecipientsService.getAll($routeParams.page, function (data) {
                $scope.recipients = data;
                $scope.pageNumber = $routeParams.page + 1;
            });
        }
    };
})(appFsMvc.Controllers = appFsMvc.Controllers || {});

