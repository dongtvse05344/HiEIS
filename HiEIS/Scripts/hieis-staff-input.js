function getRoleValues() {
    if ($('input[name="Roles"]').length === 0) {
        return null;
    }
    var checkedRoles = $('input[name="Roles"]:checked');
    var roleValues = [];
    
    if (checkedRoles.length === 0) {
        $('.error-roles').text('Vui lòng chọn ít nhất 1 vị trí.');
        roleValues = null;
    }
    else {
        $('.error-roles').text("");
        checkedRoles.each(function (index) {
            roleValues.push($(this).val());
        });
    }

    return roleValues;
}

function getStaff() {
    var roleValues = getRoleValues();

    var staff = {
        "UserName": $('#UserName').val(),
        "Password": $('#Password').val(),
        "ConfirmPassword": $('#ConfirmPassword').val(),
        "Name": $('#Name').val(),
        "Email": $('#Email').val(),
        "Code": $('#Code').val(),
        "Address": $('#Address').val(),
        "Tel": $('#Tel').val(),
        "Roles": roleValues
    };

    return staff;
}

function getUpdateStaff() {
    var roleValues = getRoleValues();

    var staff = {
        "Id": $('#edit-staff').data('staff-id'),
        "Name": $('#Name').val(),
        "AspNetUserEmail": $('#AspNetUserEmail').val(),
        "Code": $('#Code').val(),
        "Address": $('#Address').val(),
        "Tel": $('#Tel').val(),
        "Roles": roleValues
    };

    return staff;
}

$(document).ready(function () {
    toastr.options = {
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 2300
    };

    $('#create-staff').on('click', function (e) {
        e.preventDefault();
        var _button = $(this);

        var staff = getStaff();
        console.log(staff);

        $.ajax({
            url: '/Public/Staffs/CreateAsync/',
            data: staff,
            method: 'POST',
            success: function (res) {
                if (!res.success) {
                    if (res.data && res.data.length) {
                        $('#create-staff-form .field-validation-error').empty();
                        res.data.forEach(e => {
                            $('#create-staff-form [name=' + e.name + ']~.field-validation-error').html(e.error);
                        });
                        swal('Thất bại', res.message, 'warning');
                    } else {
                        swal('Thất bại', res.message, 'error');
                    }
                } else {
                    sessionStorage.setItem('message', res.message);
                    window.location.replace(res.url);
                }
            },
            fail: function () {

            }
        });



    });

    $('#edit-staff').on('click', function (e) {
        e.preventDefault();
        var _button = $(this);

        var staff = getUpdateStaff();
        console.log(staff);

        $.ajax({
            url: '/Public/Staffs/Edit/',
            data: staff,
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
                    sessionStorage.setItem('message', res.message);
                    window.location.replace(res.url);
                }
            },
            fail: function () {

            }
        });

    });
});