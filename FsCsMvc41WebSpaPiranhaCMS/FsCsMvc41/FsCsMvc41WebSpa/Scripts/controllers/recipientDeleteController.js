(function (module, $) {
    module.recipientDeleteController = function ($scope, $routeParams, RecipientsService) {
        $scope.deleteRecipient = function () {
            var id = $routeParams.smtpAddress;
            RecipientsService.deleteItem(id);
        };
    };
})(appFsMvc.Controllers = appFsMvc.Controllers || {}, jQuery);