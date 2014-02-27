(function (module) {
    module.recipientDetailController = function ($scope, RecipientsService) {
         RecipientsService.getAll(function(data) {
             $scope.recipients = data;
         });
    };
})(appFsMvc.Controllers = appFsMvc.Controllers || {});