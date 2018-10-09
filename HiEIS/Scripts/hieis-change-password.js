$(document).ready(function () {
    $('#change-password').on('click', function () {
        var accId = $(this).data('id');
        var url = $(this).data('url');

        if (url === '/Admin/Account/ChangePassword') {
            url = '/Home';
        }

        if (accId !== "") {
            var model = {
                "Id": accId,
                "Password": $('#Password').val(),
                "ConfirmPassword": $('#ConfirmPassword').val()
            };

            $.ajax({
                url: '/Admin/Account/ChangePasswordAsync/',
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
                        sessionStorage.setItem('message', res.message);
                        window.location.replace(url);
                    }
                },
                fail: function () {

                }
            });
        }
        else {
            swal('Thất bại', 'Không có tài khoản để cập nhật mật khẩu', 'error');
        }
    });
});