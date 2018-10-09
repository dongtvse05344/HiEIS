$(document).ready(function () {
    //Nhập đơn giá
    $('#UnitPrice').keyup(function (event) {

        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });

    $('#create-product').on('click', function () {
        var model = getProduct();
        console.log(model);

        $.ajax({
            url: "/Public/Products/CreateProduct/",
            data: model,
            method: "POST",
            success: function (res) {
                if (!res.success) {
                    if (res.data && res.data.length) {
                        $('#form-product .field-validation-error').empty();
                        res.data.forEach(e => {
                            $('#form-product [name=' + e.name + ']~.field-validation-error').html(e.error);
                        });
                        swal('Không thành công', res.message, 'warning');
                    } else {
                        swal('Không thành công', res.message, 'error');
                    }
                } else {
                    sessionStorage.setItem('message', res.message);
                    window.location.replace(res.url);
                }
            },
            error: function (response) {
                console.log("Fail!!!!!!!!!!!!!!!!!!!!!!!");
            }
        });
    });

    $('#edit-product').on('click', function () {
        var model = getProduct();
        console.log(model);

        $.ajax({
            url: "/Public/Products/EditProduct/",
            data: model,
            method: "POST",
            success: function (res) {
                if (!res.success) {
                    if (res.data && res.data.length) {
                        $('#form-product .field-validation-error').empty();
                        res.data.forEach(e => {
                            $('#form-product [name=' + e.name + ']~.field-validation-error').html(e.error);
                        });
                        swal('Không thành công', res.message, 'warning');
                    } else {
                        swal('Không thành công', res.message, 'error');
                    }
                } else {
                    sessionStorage.setItem('message', res.message);
                    window.location.replace(res.url);
                }
            },
            error: function (response) {
                console.log("Fail!!!!!!!!!!!!!!!!!!!!!!!");
            }
        });
    });
});

function getProduct() {
    var product = {
        "Code": $('#Code').val(),
        "Name": $('#Name').val(),
        "Unit": $('#Unit').val(),
        "UnitPrice": $('#UnitPrice').val().replace(/,/g, ""),
        "VATRate": $('#VATRate').val(),
        "HasIndex": $('#HasIndex').prop('checked'),
        "Id": $('button[type="submit"]').data('id')
    };

    return product;
}