$(document).ready(function () {
    $('#lookup-invoice').on('click', function () {
        var lookupCode = $('#InvoiceLookupCode').val();

        if (lookupCode == "") {
            swal('Vui lòng nhập mã tra cứu', '', 'warning');
        }
        else {
            var model = {
                InvoiceLookupCode: lookupCode
            };

            $.ajax({
                url: "/Public/Invoices/FindFileUrl/",
                data: model,
                method: "POST",
                success: function (res) {
                    $("#modal-form iframe").attr("src", "/Public/Invoices/FindFileUrl?InvoiceLookupCode=" + lookupCode);
                    $('#modal-form').modal('show');
                    //if (!res.success) {
                    //    if (res.data && res.data.length) {
                    //        $('.field-validation-error').empty();
                    //        res.data.forEach(e => {
                    //            $('[name=' + e.name + ']~.field-validation-error').html(e.error);
                    //        });
                    //        swal('Không thành công', res.message, 'warning');
                    //    } else {
                    //        swal('Không thành công', res.message, 'warning');
                    //    }
                    //} else {
                    //    $("#modal-form iframe").attr("src", "/Public/Invoices/FindFileUrl?InvoiceLookupCode=" + lookupCode);
                    //    $('#modal-form').modal('show');
                    //}
                },
                error: function (response) {
                    swal('Không thành công', response.message, 'warning');
                }
            });
        }

    });

    $('#lookup-proforma').on('click', function () {
        var lookupCode = $('#ProformaLookupCode').val();

        if (lookupCode == "") {
            swal('Vui lòng nhập mã tra cứu', '', 'warning');
        }
        else {
            var model = {
                LookupCode: lookupCode
            };

            console.log(model);
            $.ajax({
                url: "/Public/ProformaInvoice/GetFileUrl/",
                data: model,
                method: "POST",
                success: function (res) {
                    if (!res.success) {
                        if (res.data && res.data.length) {
                            $('.field-validation-error').empty();
                            res.data.forEach(e => {
                                $('[name=' + e.name + ']~.field-validation-error').html(e.error);
                            });
                            swal('Không thành công', res.message, 'warning');
                        } else {
                            swal('Không thành công', res.message, 'warning');
                        }
                    } else {
                        $("#modal-form iframe").attr("src", res.data);
                        $('#modal-form').modal('show');
                    }
                },
                error: function (response) {
                    console.log(response);
                    console.log("Fail!!!!!!!!!!!!!!!!!!!!!!!");
                }
            });
        }

    });
});