function scoreDetails(executionContext) {
  var formContext = executionContext.getFormContext();

  var clientURL = Xrm.Page.context.getClientUrl();

  var scorefield = formContext.getAttribute("nt_score").getValue();
  var Language = Xrm.Page.context.getUserLcid();



  if (Xrm.Page.getAttribute("nt_company_type") != null) {
    var company_type = Xrm.Page.getAttribute("nt_company_type").getValue();

  }

  var risk_assessment_code = Xrm.Page.getAttribute("nt_risk_assessment_code").getValue();
  var newTarget = "";


  if (company_type == null || company_type != "ENK") {
    Xrm.Page.getControl("nt_risk_assessment_code").setVisible(false);
    switch (scorefield) {
      case 1:
        newTarget = clientURL + "//WebResources/nt_Score1";
        break;

      case 2:
        newTarget = clientURL + "//WebResources/nt_Score2";
        break;

      case 3:
        newTarget = clientURL + "//WebResources/nt_Score3";
        break;

      case 4:
        newTarget = clientURL + "//WebResources/nt_Score4";
        break;

      case 5:
        newTarget = clientURL + "//WebResources/nt_Score5";
        break;

      case 6:
        newTarget = clientURL + "//WebResources/nt_Score6";
        break;

      case 7:
        newTarget = clientURL + "//WebResources/nt_Score7";
        break;

      case 8:
        newTarget = clientURL + "//WebResources/nt_Score8";
        break;

      case 9:
        newTarget = clientURL + "//WebResources/nt_Score9";
        break;

      case 10:
        newTarget = clientURL + "//WebResources/nt_Score10";
        break;

      default:
        newTarget = clientURL + "//WebResources/nt_Score0";
        break;
    }
  }


  if (company_type != null) {
    if (company_type == "ENK") {

      Xrm.Page.getControl("nt_risk_assessment_code").setVisible(true);


      if (Language == 1033) {
        switch (risk_assessment_code) {
          case 1:
            newTarget = clientURL + "//WebResources/nt_RiskCodeHigh";
            break;

          case 2:
            newTarget = clientURL + "//WebResources/nt_RiskCodeMedium";
            break;

          case 3:
            newTarget = clientURL + "//WebResources/nt_RiskCodeLow";
            break;

          default:
            newTarget = clientURL + "//WebResources/nt_Score0";
            break;
        }
      }




      if (Language == 1030) {
        switch (risk_assessment_code) {
          case 1:
            newTarget = clientURL + "//WebResources/nt_RiskCodeHighDk";
            break;

          case 2:
            newTarget = clientURL + "//WebResources/nt_RiskCodeMeduimDk";
            break;

          case 3:
            newTarget = clientURL + "//WebResources/nt_RisikaCodeLowDk";
            break;

          default:
            newTarget = clientURL + "//WebResources/nt_Score0";
            break;
        }


      }
    }
  }


  var IFrame = formContext.ui.controls.get("IFRAME_Score");

  IFrame.setSrc(newTarget);

}