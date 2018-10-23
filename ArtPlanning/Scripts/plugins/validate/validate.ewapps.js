    (function ($) {
        var defaultOptions = {
            ignore: ".ignore",
            errorClass: 'help-block',
            highlight: function (element) {
                $(element)
                  .closest('.form-group')
                  .addClass('has-error');
            },
            unhighlight: function (element) {
                $(element)
                  .closest('.form-group')
                  .removeClass('has-error');
            },
            errorPlacement: function (error, element) {
                if (element.prop('type') === 'checkbox') {
                    error.insertAfter(element.parent().parent().parent());
                } else {
                    error.insertAfter(element.parent());
                }
            }
        };
        $.validator.setDefaults(defaultOptions);

        $.validator.unobtrusive.options = {
            errorClass: defaultOptions.errorClass,
            validClass: defaultOptions.validClass,
        };
    })(jQuery);
