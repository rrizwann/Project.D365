function OpenWebResource() {
    Xrm.Utility.openWebResource("nt_/controls/companysearch/index.html");
} 

function OpenDashboard() {
    var number = Xrm.Page.getAttribute("accountnumber").getValue();
    Xrm.Navigation.openUrl("https://dashboard.risika.dk/creditcheck/dk/" + number); 
} 

function IsEnableDashboard() {
    var number = Xrm.Page.getAttribute("accountnumber").getValue();
    if (number) {
        return true;
    } 
    return false;
} 

function RefreshWebResource() {
    var webResourceControl = window.parent.Xrm.Page.getControl("nt_RisikaScore");
    var src = webResourceControl.getSrc();
    webResourceControl.setSrc(null);
    webResourceControl.setSrc(src);
}


function RunRisikaWorkflow() {
  debugger;
  Xrm.Utility.showProgressIndicator("Opdaterer...");
  var entityIdforRefresh = Xrm.Page.data.entity.getId().replace("{", '"').replace("}", '"');
  var entityId = Xrm.Page.data.entity.getId().replace("{", "").replace("}", "");
  var entity = {
    "EntityId": entityId
  };



  var entityName = Xrm.Page.data.entity.getEntityName();



  var WorkflowId = "4A23E585-87B5-4E02-886F-D344E9409A7A";


  var req = new XMLHttpRequest();

  req.open("POST", Xrm.Page.context.getClientUrl() + "/api/data/v9.1/workflows(" + WorkflowId + ")/Microsoft.Dynamics.CRM.ExecuteWorkflow", true);

  req.setRequestHeader("OData-MaxVersion", "4.0");

  req.setRequestHeader("OData-Version", "4.0");

  req.setRequestHeader("Accept", "application/json");

  req.setRequestHeader("Content-Type", "application/json; charset=utf-8");

  req.onreadystatechange = function () {

    if (this.readyState === 4) {
      req.onreadystatechange = null;

      if (this.status === 200)
        Xrm.Utility.alertDialog("Sucess");
      Xrm.Utility.closeProgressIndicator;

      setTimeout(function () { Xrm.Utility.openEntityForm(Xrm.Page.data.entity.getEntityName(), Xrm.Page.data.entity.getId()); }, 6000);

    }
  }

  req.send(JSON.stringify(entity));

}


