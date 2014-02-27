(function (module, $) {
    module.recipientCreateController = function ($scope, RecipientsService) {
        $scope.addRecipient = function () {
            var data = appFsMvc.utility.serializeObject($("#recipientForm"));
            RecipientsService.addItem(data);
        };
    };
})(appFsMvc.Controllers = appFsMvc.Controllers || {}, jQuery);