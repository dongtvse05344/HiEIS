$(document).ready(function (e) {

    $('#save-staff').on('click', function () {
        var _button = $(this);

        var model = {
            "Id": _button.data('id'),
            "Name": $('#Name').val(),
            "AspNetUserEmail": $('#AspNetUserEmail').val(),
            "Code": $('#Code').val(),
            "Address": $('#Address').val(),
            "Tel": $('#Tel').val(),
            "Roles": ""
        };

        $.ajax({
            url: '/Public/Account/EditStaff/',
            data: model,
            method: 'POST',
            success: function (res) {
                if (!res.success) {
                    if (res.data && res.data.length) {
                        $('#edit-staff-form .field-validation-error').empty();
                        res.data.forEach(e => {
                            $('#edit-staff-form [name=' + e.name + ']~.field-validation-error').html(e.error);
                        });
                        swal('Thất bại', res.message, 'warning');
                    } else {
                        swal('Thất bại', res.message, 'error');
                    }
                } else {
                    $('.field-validation-error').html('')
                    swal('Thành công', res.message, 'success');
                }
            },
            fail: function () {

            }
        });
    });

    $('#save-customer').on('click', function () {
        var _button = $(this);

        var model = {
            "Id": _button.data('id'),
            "UserName": $('#UserName').val(),
            "CustomerName": $('#CustomerName').val(),
            "Email": $('#Email').val(),
            "CustomerEnterprise": $('#CustomerEnterprise').val(),
            "CustomerTaxNo": $('#CustomerTaxNo').val(),
            "CustomerAddress": $('#CustomerAddress').val(),
            "CustomerTel": $('#CustomerTel').val(),
            "CustomerFax": $('#CustomerFax').val(),
            "CustomerBank": $('#CustomerBank').val(),
            "CustomerBankAccountNumber": $('#CustomerBankAccountNumber').val()
        };

        $.ajax({
            url: '/Public/Account/EditCustomer/',
            data: model,
            method: 'POST',
            success: function (res) {
                if (!res.success) {
                    if (res.data && res.data.length) {
                        $('#edit-customer-form .field-validation-error').empty();
                        res.data.forEach(e => {
                            $('#edit-customer-form [name=' + e.name + ']~.field-validation-error').html(e.error);
                        });
                        swal('Thất bại', res.message, 'warning');
                    } else {
                        swal('Thất bại', res.message, 'error');
                    }
                } else {
                    $('.field-validation-error').html('')
                    swal('Thành công', res.message, 'success');
                }
            },
            fail: function () {

            }
        });
    });

    var url = $('#change-password').data('url');
    $('#change-password').on('click', function () {

        var model = {
            "Password": $('#Password').val(),
            "ConfirmPassword": $('#ConfirmPassword').val()
        };

        $.ajax({
            url: '/Public/Account/ChangePasswordAsync/',
            data: model,
            method: 'POST',
            success: function (res) {
                if (!res.success) {
                    if (res.data && res.data.length) {
                        $('#password-form .field-validation-error').empty();
                        res.data.forEach(e => {
                            $('#password-form [name=' + e.name + ']~.field-validation-error').html(e.error);
                        });
                        swal('Thất bại', res.message, 'warning');
                    } else {
                        swal('Thất bại', res.message, 'error');
                    }
                } else {
                    swal('Thành công', res.message, 'success');
                    $('#Password, #ConfirmPassword').val('');
                    $('.field-validation-error').text('');
                }
            },
            fail: function () {

            }
        });
    });
});