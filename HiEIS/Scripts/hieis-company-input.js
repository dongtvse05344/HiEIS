function getCompany() {

    var company = {
        Name: $('#Name').val(),
        TaxNo: $('#TaxNo').val(),
        Address: $('#Address').val(),
        Tel: $('#Tel').val(),
        Email: $('#Email').val(),
        Website: $('#Website').val(),
        Fax: $('#Fax').val(),
        Bank: $('#Bank').val(),
        BankAccountNumber: $('#BankAccountNumber').val(),
        Id: $('.js-update').data('company-id')
    };

    return company;
}

$(document).ready(function () {

    //Lấy MST
    $("#btn-tax-no").on("click", function () {
        var tax = $("#TaxNo").val();
        if (tax === "") {
            toastr.warning("Vui lòng nhập mã số thuế.", "Nhắc nhở");
        }
        else {
            $.ajax({
                url: '/Admin/Company/GetEnterpriseInfoByTaxNo',
                data: { taxNo: tax },
                method: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (response) {
                    if (response.IsDelete === true || response.MaSoThue === null) {
                        toastr.error("Mã số thuế không tồn tại.", "Không thành công");
                        $("#Name").val("");
                        $("#Address").val("");
                    }
                    else {
                        $("#TaxNo").val(response.MaSoThue);
                        $("#Name").val(response.Title);
                        $("#Address").val(response.DiaChiCongTy);
                    }
                },
                error: function (response) {
                    toastr.error("Có lỗi xảy ra, vui lòng thử lại sau.", "Không thành công");
                }
            });
        }

    });
    
    $('.company-form').on('click', '.js-create', function (e) {
        e.preventDefault();
        var _button = $(this);

        var company = getCompany();
        console.log(company);

        $.ajax({
            url: '/Admin/Company/CreateAsync/',
            data: company,
            method: 'POST',
            success: function (res) {
                if (!res.success) {
                    if (res.data && res.data.length) {
                        $('.company-form .field-validation-error').empty();
                        res.data.forEach(e => {
                            $('.company-form [name=' + e.name + ']~.field-validation-error').html(e.error);
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

    $('.company-form').on('click', '.js-update', function (e) {
        e.preventDefault();

        var company = getCompany();
        var companyId = parseInt($(this).attr("data-company-id"));

        $.ajax({
            url: '/Admin/Company/Edit/',
            data: company,
            method: 'POST',
            success: function (res) {
                if (!res.success) {
                    if (res.data && res.data.length) {
                        $('.company-form .field-validation-error').empty();
                        res.data.forEach(e => {
                            $('.company-form [name=' + e.name + ']~.field-validation-error').html(e.error);
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