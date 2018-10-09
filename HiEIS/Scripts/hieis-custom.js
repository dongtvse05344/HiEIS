window.APP_PUBLIC = {};

/* Region: custom helper functions for DataTable */
APP_PUBLIC.DataTableCustom = function () {

    var initDataTable = function (tableId, container, downloadOptions, filterOptions, ajaxUrl, columnOptions, columnDefOptions, callback) {
        //Init table
        var table = $(tableId).DataTable({
            ajax: {
                url: ajaxUrl,
                dataSrc: ''
            },
            pageLength: 10,
            responsive: true,
            autoWidth: false,
            dom: 'Bfrtip',
            columns: columnOptions,
            buttons: downloadOptions,
            columnDefs: columnDefOptions,
            initComplete: function (settings, json) {
                $(container).find('#search-name').on('keyup', function () {
                    table.columns(0).search(this.value).draw();
                });

                $(container).find('#search-code').on('keyup', function () {
                    table.columns(1).search(this.value).draw();
                });

                if (filterOptions.payment !== undefined) {
                    $(container).find('#search-payment').on('change', function () {
                        table.columns(filterOptions.payment).search(this.value).draw();
                    });
                }

                if (filterOptions.status !== undefined) {
                    $(container).find('#search-status').on('change', function () {
                        table.columns(filterOptions.status).search(this.value).draw();
                    });
                }

                if (filterOptions.date !== undefined) {
                    $.fn.dataTable.ext.search.push(
                        function (settings, data, dataIndex) {
                            const DATE_FORMAT = 'DD/MM/YYYY';
                            var $min = $('#min');
                            var $max = $('#max');

                            var minDate = moment('01/01/1900'), maxDate = moment('01/01/9999');
                            if ($min.val() !== '')
                                minDate = moment($min.datepicker('getDate'), DATE_FORMAT);

                            if ($max.val() !== '')
                                maxDate = moment($max.datepicker('getDate'), DATE_FORMAT);

                            var currDate = moment(data[filterOptions.date], DATE_FORMAT);
                            return minDate <= currDate && currDate <= maxDate;
                        }
                    );
                }

                if (filterOptions.price !== undefined) {
                    $.fn.dataTable.ext.search.push(
                        function (settings, data, dataIndex) {
                            var min = parseFloat($('#search-min').val().replace(/,/g, ""), 10);
                            var max = parseFloat($('#search-max').val().replace(/,/g, ""), 10);
                            var value = parseFloat(data[filterOptions.price].replace(/\,/g, '')) || 0; // use data for the value column

                            if ((isNaN(min) && isNaN(max)) ||
                                (isNaN(min) && value <= max) ||
                                (min <= value && isNaN(max)) ||
                                (min <= value && value <= max)) {
                                return true;
                            }
                            return false;
                        }
                    );
                }

                //if (callback !== undefined)
                //    callback();
            }
        });

        //Init datepicker
        $(container).find('#min, #max').datepicker({
            format: 'dd/mm/yyyy',
            keyboardNavigation: false,
            forceParse: false,
            autoclose: true,
            onSelect: function () {
                table.draw();
            }
        });

        $(container).find('#min, #max').change(function () {
            table.draw();
        });

        $(container).find('#search-min, #search-max').on('keyup', function () {
            table.draw();
        });
    }

    return {
        initDataTable: initDataTable
    }

}();

function moveComponents() {
    $('.dt-buttons').appendTo($('.table-button-group .left'));
    $('.table-pagination').appendTo($('.dataTables_wrapper'));
    $('.dataTables_info').appendTo($('.table-pagination .left'));
    $('.dataTables_paginate').appendTo($('.table-pagination .right'));
}

function pad(number) {
    var str = '' + number;
    while (str.length < 6) {
        str = '0' + str;
    }
    return str;
}

function numberWithCommas(number) {
    var parts = number.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

$(document).ready(function () {
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });

    var currentUrl = window.location.pathname;
    var groups = currentUrl.split("/");
    var groupName = groups[2];

    $activeItem = $('[data-group="' + groupName.toLowerCase() + '"]');
    $activeItem.addClass('active');

    if ($activeItem.children('.submenu').length > 0) {        
        $submenu = $activeItem.children('.submenu');

        groupName = groups[3].replace(/Edit|Create/ig, "");
        $('[data-group="' + groupName.toLowerCase() + '"]').addClass('active');
        $submenu.addClass('in');
        $activeItem.css('background', '#293846');
    }

});
