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

function getStaffAccount() {
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
        "Roles": roleValues,
        "CompanyId": parseInt($('#CompanyId').val())
    };

    return staff;
}

function getUpdateStaffAccount() {
    var roleValues = getRoleValues();

    var staff = {
        "Id": $('#js-edit-staff').attr("data-id"),
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
    $('#CompanyName').typeahead(
        {
            highlight: true
        },
        {
            name: 'company-name',
            display: function (item) {
                return item.Name;
            },
            source: function (query, sync, callback) {
                $.ajax({
                    url: '/Admin/Company/List',
                    method: 'POST',
                    dataType: "json",
                    data: {
                        searchPhase: query,
                        page: 0,
                        pageSize: 10
                    },
                    success: function (res) {
                        callback(res.data);
                    }
                });
                return false;
            }
        }).bind("typeahead:select", function (e, company) {
            $('#CompanyId').val(company.Id);
        });

    //Create staff account
    $('#js-create-staff').on('click', function (e) {
        e.preventDefault();
        var _button = $(this);

        var model = getStaffAccount();
        console.log(model);

        $.ajax({
            url: '/Admin/Account/CreateStaffAsync/',
            data: model,
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

    //Update staff account
    $('#js-edit-staff').on('click', function (e) {
        e.preventDefault();
        var _button = $(this);

        var model = getUpdateStaffAccount();
        console.log(model);

        $.ajax({
            url: '/Admin/Account/EditStaffAsync/',
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
                    sessionStorage.setItem('message', res.message);
                    window.location.replace(res.url);
                }
            },
            fail: function () {

            }
        });

    });
});