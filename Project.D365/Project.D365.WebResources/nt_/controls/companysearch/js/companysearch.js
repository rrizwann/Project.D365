var table;
var oXrmForm;
$(document).ready(function () {

    function Init() {

        oXrmForm = window.parent.opener;
        table = $('#datatable').DataTable({
            destroy: false,
            paging: true,
            sort: true,
            //order: [[1, "asc"]],
            searching: false,
            scrollY: 330,
            columnDefs: [
                { className: "dt-left", targets: '_all' }
            ],
            columns: [
                { 'data': 'CVR', 'width': '15%' },
                { 'data': 'Name', 'width': '35%', },
                { 'data': 'Address', 'width': '40%' },
                { 'data': 'Status', 'width': '10%' }
            ],
            language: {
                lengthMenu: "Viser _MENU_ firmaer",
                emptyTable: "Listen er tom",
                zeroRecords: "Listen er tom",
                info: "Viser _START_ til _END_ af _TOTAL_ firmaer fundet",
                infoEmpty: "Viser 0 til 0 af 0 firmaer fundet",
                paginate: {
                    previous: 'Forrige',
                    next: 'Næste'
                }
            }
        });

        $('#datatable tbody').on('click', 'tr', function () {
            if (oXrmForm) {
                var value = table.row(this).data();
                if (value) {
                    if (value.CVR) {
                        var ctrlNumber = oXrmForm.Xrm.Page.getAttribute("accountnumber");
                        if (ctrlNumber) {
                            ctrlNumber.setValue(value.CVR.toString());
                        }
                    }
                    
                    if (value.Name)
                    var ctrlName = oXrmForm.Xrm.Page.getAttribute("name");
                    if (ctrlName) {
                        ctrlName.setValue(value.Name);
                    }

                    oXrmForm.Xrm.Page.data.entity.save();
                    window.close();
                } 
            }
        });

        BindEvents();
    }

    function BindEvents() {

        $("#btnFind").click(Load);
    }

    function Load() {

        var $txtCompany = $("#txtCompany");
        if (IsBlank($txtCompany)) {

            $txtCompany.focus();
            alert('Indtast Firma navn for at hente firmaer oplysninger.');
        }
        else {
            BlockUI();
            if ($txtCompany) {
                var valCompany = $txtCompany.val();
                if (valCompany != null) {
                    GetCompanies(valCompany);
                }
            }
            UnBlockUI();
        }
    }

    function GetCompanies(value) {

        var clientURL = Xrm.Page.context.getClientUrl();
        var query = "/api/data/v9.1/nt_RetrieveCompanies";

        var req = new XMLHttpRequest();
        req.open("POST", encodeURI(clientURL + query), false);
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                req.onreadystatechange = null;
                if (this.status === 200) {
                    var oresponse = JSON.parse(this.response);
                    var collection = GetCollection(oresponse);
                    BindData(collection);
                } else {
                    Xrm.Utility.alertDialog(this.statusText);
                }
            }
        };

        var parameters = {};
        parameters.Url = "https://api.risika.dk/v1.2/";
        parameters.Token = "JWT eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6NzU4LCJ0eXBlIjoic2luZ2xlLXVzZXIiLCJncmFudF9wZXJtaXQiOm51bGwsImxhbmd1YWdlIjoiZGFfREsiLCJjb21wYW55IjoxODgsImlhdCI6MTU1NjU2NjgxMywibmJmIjoxNTU2NTY2ODEzLCJleHAiOjE1ODgxODkyMTN9.wRO3F4yY_Z-4IDlwQw3JbmoX2IlVSWYdo4EhS8LN1y8";
        parameters.Name = value;

        req.send(JSON.stringify(parameters));
    }

    function GetCollection(oresponse) {

        var collection = {
            rows: []
        };

        var items = JSON.parse(oresponse.Response).search_result;

        for (var i in items) {

            var item = items[i];

            var cvr = i;
            if (!IsNullVale(item.local_organization_id)) {
                if (!IsNullVale(item.local_organization_id.id)) {
                    cvr = item.local_organization_id.id;
                }
            }

            var name = '';
            if (typeof item.company_name !== 'undefined') {
                name = item.company_name;
            }

            var address = '';
            if (typeof item.address !== 'undefined') {
                if (!IsNullVale(item.address.coname)) {
                    address += (item.address.coname + " - ");
                }
                if (!IsNullVale(item.address.number)) {
                    address += (item.address.number + " ");
                }
                if (!IsNullVale(item.address.street)) {
                    address += (item.address.street + ", ");
                }
                if (!IsNullVale(item.address.municipality)) {
                    address += (item.address.municipality + ", ");
                }
                if (!IsNullVale(item.address.city)) {
                    address += (item.address.city + ", ");
                }
                if (!IsNullVale(item.address.postdistrict)) {
                    address += (item.address.postdistrict + ", ");
                }
                if (!IsNullVale(item.address.zipcode)) {
                    address += (item.address.zipcode + ", ");
                }
                if (typeof item.address.country !== 'undefined') {
                    address += (item.address.country + ", ");
                }
                address = address.replace(/,\s*$/, "");
            }

            var status = '';
            if (typeof item.active !== 'undefined') {
                if (item.active) {
                    status = "Active";
                }
                else {
                    status = "Inactive";
                }
            }

            collection.rows.push({
                "CVR": cvr,
                "Name": name,
                "Address": address,
                "Status": status
            });
        }

        return collection;
    }

    function BindData(collection) {
        table.clear().rows.add(collection.rows).draw();
    }

    function IsNullVale(value) {
        if (typeof value === 'undefined' || value === null) {
            return true;
        }
        return false;
    }

    function IsBlank(ctrl) {

        var value = ctrl.val();
        return (!value || /^\s*$/.test(value));
    }

    function BlockUI() {
        $.blockUI({
            message: $('#processing'),
            css: {
                border: 'none',
                backgroundColor: '#fff',
                opacity: .5
            },
            overlayCSS: {
                border: 'none',
                backgroundColor: '#fff',
                opacity: 0.5
            },
        });
    }

    function UnBlockUI() {
        $.unblockUI();
    }

    Init(); 
});