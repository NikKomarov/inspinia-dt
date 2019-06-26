var orderListModule = (function () {

    self.init = function () {
        $(document).ready(function () {
            $('.dataTables-example').DataTable({
                responsive: true,
                serverSide: true,
                "ajax": {
                    "url": "/order/GetDatatable",
                    "type": "POST"
                },
                columns: [
                    { data: "id" },
                    {
                        data: "status", render: function (data, type, row) {
                            return `<span class="label label_status" status="${data}">${data}</span>`
                        },
                        "className": "text-center",
                    },
                    { data: "client" },
                    { data: "email" },
                    { data: "sum", "className": "text-right", }
                ],
                dom: '<"html5buttons"B>lTfgitp',
                buttons: [
                    { extend: 'copy' },
                    { extend: 'csv' },
                    { extend: 'excel', title: 'ExampleFile' },
                    { extend: 'pdf', title: 'ExampleFile' },

                    {
                        extend: 'print',
                        customize: function (win) {
                            $(win.document.body).addClass('white-bg');
                            $(win.document.body).css('font-size', '10px');

                            $(win.document.body).find('table')
                                .addClass('compact')
                                .css('font-size', 'inherit');
                        }
                    }
                ]

            });

        });
    };

    // public members
    return {
        init: init
    };

}());
