﻿var isTest = false;
var subscriptions = subscription;
var subToken = token;
var first = '.l2';
var jsonPass = '';
var propArray = new Array();
var edgeArray = new Array();
var firstRes = true;
var cy;
var permission = new Array();
//permission[0] = 'Subject';
//permission[1]='To';

//replace the " for ', because " is being used for the label of the node
function replaceAll(txt) {
    while (txt.includes('"')) {
        txt = txt.replace('"', "'");
    }
    return txt;
}

//waits the ms amount of time
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

//Get the actual size of the divs, and set the size of the node, the 't' parameter is for the first
//time to wait for the node to be render
async function resizeFlow(t) {
    await sleep(t);
    for (var i = 0; i < edgeArray.length; i++) {
        var divH = 0;
        var divW = 0;
        var docH = document.getElementById(edgeArray[i][0]);
        var docW = document.getElementById(edgeArray[i][0]);

        if (docH != null) divH = docH.offsetHeight;
        if (docW != null) divW = docW.offsetWidth;
        if (divH > 0 && divW > 0) {
            cy.getElementById(edgeArray[i][0]).style('height', divH + 10);
            cy.getElementById(edgeArray[i][0]).style('width', divW + 10);
            firstRes = false;
            cy.fit();
            cy.maxZoom(6);
            cy.minZoom(0.5);
        }
    }
}

//set focus on the node that is clicked
async function getFocus(a) {
    await sleep(10);
    var fc = document.getElementById(a.id);
    if (fc != null) fc.focus();
}

function logout() {
    window.location = "Login.aspx";
}

//class Login extends React.Component {
//    constructor() {
//        super();
//        this.state = {
//            userType:[]
//        }
//    };
//    render() {
//        return (
//            <form id="form1" runat="server">
//                <div class="login-page">
//                    <div class="form">
//                        <asp: TextBox placeholder="Username" CssClass="input" ID="txtUser" runat="server"></asp: TextBox>
//                        <asp: TextBox placeholder="Password" CssClass="input" ID="txtPass" runat="server" TextMode="Password"></asp: TextBox>
//                        <asp: Button CssClass="btn" ID="Button1" runat="server" OnClick="Button1_Click" Text="Log In" />
//                    </div>
//                </div >
//            </form >
//            );
//    }
//}

class WorkflowList extends React.Component {
    constructor() {
        super();
        this.state = {
            coininfo: [],
            token1: subToken,
            selectedWorkflowId: '/subscriptions/' + subscriptions + '/resourceGroups/testLogicapp/providers/Microsoft.Logic/workflows/test?api-version=2016-06-01',
        };
        this.AddAlerts = this.AddAlert.bind(this);
    };
    render(props) {
        return (
            <table className="table">
                <thead>
                    <tr>
                        <th width="100px"  >Name</th>
                        <th width="300px"  >Update</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.coininfo}
                </tbody>
            </table>
        );
    }
    handleChange(event) {

    }
    AddAlert2() {
        console.log("AddAlert .....");
        var arr = this.state.myAlerts;
        arr.splice(0, 0, { "id": "ripple", "amount": 2 });
        this.setState({ myAlerts: arr });
    }
    eachWorkflow(pic) {
        return (
            <tr>
                <td>
                    {pic.name}
                </td>
                <td>
                    <AddAlertButton jsonTxtAdd={pic} AddAlerts={this.AddAlerts} jsonName={pic.name} />
                </td>
            </tr>
        )
    }
    AddAlert(picJsn) {
        debugger;
        console.log("AddAlert .....");
        console.log(picJsn);
        fetch('https://management.azure.com' + picJsn.id + '?api-version=2016-06-01',
            {
                method: 'PUT',
                headers: {
                    'Authorization': 'Bearer ' + this.state.token1,
                    'Content-type': 'application/json'
                },
                body: JSON.stringify(picJsn),
            })
            .then(results => {
                if (results.status < 400) {
                    swal({
                        title: "Workflow saved!",
                        text: "Workflow saved successfully!",
                        icon: "success",
                        button: "Ok",
                    }).then(function () {
                        window.location.reload();
                    });
                } else {
                    console.log(results);
                    swal({
                        title: "Workflow not saved!",
                        text: "Try again later!",
                        icon: "error",
                        button: "Ok",
                    }).then(function () {
                        window.location.reload();
                    });
                }
                return results.json();
            }).then(data => {
                console.log(data);
            });
    }
    componentDidMount() {

        fetch('https://management.azure.com/subscriptions/' + subscriptions + '/providers/Microsoft.Logic/workflows?api-version=2016-06-01',
            {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + this.state.token1,
                }
            }).then(results => {
                return results.json();
            }).then(data => {
                console.log(data.value[0].name);
                let coininfo = data.value.map(this.eachWorkflow.bind(this));
                this.setState({ coininfo: coininfo });
            });
    }
}

class AddAlertButton extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: '',
            jsonName: '',
            showing: true,
        };
        this.changeData = this.handleChangeData.bind(this);
    };
    UpdateJsonAlert(json) {
        this.props.AddAlerts(json);
    }
    handleState() {
        this.setState({
            showing: false,
        });
    }
    async handleChangeData() {
        var id1 = document.getElementById('loaderId');
        var id2 = document.getElementById('cyId');
        var id3 = document.getElementById('exampleModal3Label');
        var id4 = document.getElementById('btn_save');
        id2.removeAttribute("hidden");
        id1.setAttribute("hidden", true);
        id4.removeAttribute("disabled");
        this.setState({
            data: this.props.jsonTxtAdd,
            jsonName: this.props.jsonName,
            showing: true,
        });
        id3.innerText = this.props.jsonName;
        await sleep(5);
        this.forceUpdate();
    }
    render() {
        const { showing } = this.state;
        return (
            <div className="d-inline bg-primary">
                <button type="button" className="btn btn-primary" data-toggle="modal" data-target="#exampleModal4" onClick={this.changeData}>
                    View
                </button>
                <div className="modal fade" id="exampleModal4" tabIndex="-1" role="dialog" aria-labelledby="exampleModal3Label" aria-hidden="true">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title" id="exampleModal3Label"></h5>
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">×</span>
                                </button>
                            </div>
                            <div className="modal-body">
                                <div id="loaderId" class="loader-container" hidden >
                                    <div class="loader"></div>
                                </div>
                                <div id="cyId">
                                    <div id="cy"></div>
                                </div>
                            </div>
                            <div className="modal-footer">
                                <div className="startButton">
                                    <button className="btn btn-primary" onClick={() => resizeFlow(0)}>Center</button>
                                </div>
                                <button type="button" className="btn btn-secondary" data-dismiss="modal">Close</button>
                                <FlowDiagarm jsonTxt={this.state.data} UpdateJsonAlert={this.UpdateJsonAlert.bind(this)} handleState={this.handleState.bind(this)} />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            test: isTest,
        };
    }


    render() {
        if (this.state.test) {
            return (
                <div  >
                    <AddAlertButton jsonName={'test1'} jsonTxtAdd={{ "properties": { "provisioningState": "Succeeded", "createdTime": "2019-11-05T15:47:53.1442395Z", "changedTime": "2019-11-13T14:32:53.630449Z", "state": "Enabled", "version": "08586279513118658578", "accessEndpoint": "https://prod-56.eastus.logic.azure.com:443/workflows/729de46cc26c4573838688f1c944b060", "definition": { "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#", "contentVersion": "1.0.0.0", "parameters": { "$connections": { "defaultValue": {}, "type": "Object" } }, "triggers": { "manual": { "type": "Request", "kind": "Http", "inputs": { "schema": {} } } }, "actions": { "Send_an_email_(V2)": { "runAfter": {}, "type": "ApiConnection", "inputs": { "body": { "Body": "this is a test message", "Subject": "test message apm", "To": "damian@sinay.com.ar" }, "host": { "connection": { "name": "@parameters('$connections')['office365']['connectionId']" } }, "method": "post", "path": "Send_an_email_(V3)" } }, "Send_an_email_(V3)": { "runAfter": { "Send_an_email_(V2)": ["Succeeded"] }, "type": "ApiConnection", "inputs": { "body": { "Body": "this is a test message", "Subject": "test message apm", "To": "damian@sinay.com.ar" }, "host": { "connection": { "name": "@parameters('$connections')['office365']['connectionId']" } }, "method": "post", "path": "Send_an_email_(V2)" } } }, "outputs": {} }, "parameters": { "$connections": { "value": { "office365": { "connectionId": "/subscriptions/0d78362b-5b0f-4a4f-ae7e-4445ab90efaf/resourceGroups/APM/providers/Microsoft.Web/connections/office365", "connectionName": "office365", "id": "/subscriptions/0d78362b-5b0f-4a4f-ae7e-4445ab90efaf/providers/Microsoft.Web/locations/eastus/managedApis/office365" } } } }, "endpointsConfiguration": { "workflow": { "outgoingIpAddresses": [{ "address": "13.92.98.111" }, { "address": "40.121.91.41" }, { "address": "40.114.82.191" }, { "address": "23.101.139.153" }, { "address": "23.100.29.190" }, { "address": "23.101.136.201" }, { "address": "104.45.153.81" }, { "address": "23.101.132.208" }], "accessEndpointIpAddresses": [{ "address": "137.135.106.54" }, { "address": "40.117.99.79" }, { "address": "40.117.100.228" }, { "address": "137.116.126.165" }] }, "connector": { "outgoingIpAddresses": [{ "address": "40.71.11.80/28" }, { "address": "40.71.249.205" }, { "address": "191.237.41.52" }] } } }, "id": "/subscriptions/0d78362b-5b0f-4a4f-ae7e-4445ab90efaf/resourceGroups/APM/providers/Microsoft.Logic/workflows/test", "name": "test", "type": "Microsoft.Logic/workflows", "location": "eastus", "tags": { "permission": "111" } }} />
                    <br /> workflows<br />
                    <AddAlertButton jsonName={'test2'} jsonTxtAdd={{ "properties": { "provisioningState": "Succeeded", "createdTime": "2019-11-05T15:47:53.1442395Z", "changedTime": "2019-11-13T14:32:53.630449Z", "state": "Enabled", "version": "08586279513118658578", "accessEndpoint": "https://prod-56.eastus.logic.azure.com:443/workflows/729de46cc26c4573838688f1c944b060", "definition": { "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#", "contentVersion": "1.0.0.0", "parameters": { "$connections": { "defaultValue": {}, "type": "Object" } }, "triggers": { "manual": { "type": "Request", "kind": "Http", "inputs": { "schema": {} } } }, "actions": { "Send_an_email_(V2)": { "runAfter": {}, "type": "ApiConnection", "inputs": { "body": { "Body": "this is a test message", "Subject": "test message apm", "To": "damian@sinay.com.ar" }, "host": { "connection": { "name": "@parameters('$connections')['office365']['connectionId']" } }, "method": "post", "path": "Send_an_email_(V3)" } }, "Send_an_email_(V3)": { "runAfter": { "Send_an_email_(V2)": ["Succeeded"] }, "type": "ApiConnection", "inputs": { "body": { "Body": "this is a test message", "Subject": "test message apm", "To": "damian@sinay.com.ar" }, "host": { "connection": { "name": "@parameters('$connections')['office365']['connectionId']" } }, "method": "post", "path": "Send_an_email_(V2)" } } }, "outputs": {} }, "parameters": { "$connections": { "value": { "office365": { "connectionId": "/subscriptions/0d78362b-5b0f-4a4f-ae7e-4445ab90efaf/resourceGroups/APM/providers/Microsoft.Web/connections/office365", "connectionName": "office365", "id": "/subscriptions/0d78362b-5b0f-4a4f-ae7e-4445ab90efaf/providers/Microsoft.Web/locations/eastus/managedApis/office365" } } } }, "endpointsConfiguration": { "workflow": { "outgoingIpAddresses": [{ "address": "13.92.98.111" }, { "address": "40.121.91.41" }, { "address": "40.114.82.191" }, { "address": "23.101.139.153" }, { "address": "23.100.29.190" }, { "address": "23.101.136.201" }, { "address": "104.45.153.81" }, { "address": "23.101.132.208" }], "accessEndpointIpAddresses": [{ "address": "137.135.106.54" }, { "address": "40.117.99.79" }, { "address": "40.117.100.228" }, { "address": "137.116.126.165" }] }, "connector": { "outgoingIpAddresses": [{ "address": "40.71.11.80/28" }, { "address": "40.71.249.205" }, { "address": "191.237.41.52" }] } } }, "id": "/subscriptions/0d78362b-5b0f-4a4f-ae7e-4445ab90efaf/resourceGroups/APM/providers/Microsoft.Logic/workflows/test", "name": "test", "type": "Microsoft.Logic/workflows", "location": "eastus", "tags": { "permission": "111" } }} />
                    <br />
                    <AddAlertButton jsonName={'test3'} jsonTxtAdd={{ "properties": { "provisioningState": "Succeeded", "createdTime": "2019-12-05T18:31:58.0043722Z", "changedTime": "2019-12-05T18:59:36.5016788Z", "state": "Enabled", "version": "08586260345090213227", "accessEndpoint": "https://prod-31.centralus.logic.azure.com:443/workflows/5d92d13bbfb7406799dbcaefbaf19cfe", "definition": { "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#", "contentVersion": "1.0.0.0", "parameters": {}, "triggers": { "manual": { "type": "Request", "kind": "Http", "inputs": { "method": "GET", "schema": {} } } }, "actions": { "Condition": { "actions": { "Response": { "runAfter": {}, "type": "Response", "kind": "Http", "inputs": { "body": "came parameter A", "statusCode": 200 } } }, "runAfter": {}, "else": { "actions": { "Response_2": { "runAfter": {}, "type": "Response", "kind": "Http", "inputs": { "body": "no params", "statusCode": 200 } } } }, "expression": { "and": [{ "equals": ["@triggerBody()", "A"] }] }, "type": "If" } }, "outputs": {} }, "parameters": {}, "endpointsConfiguration": { "workflow": { "outgoingIpAddresses": [{ "address": "13.67.236.125" }, { "address": "104.208.25.27" }, { "address": "40.122.170.198" }, { "address": "40.113.218.230" }, { "address": "23.100.86.139" }, { "address": "23.100.87.24" }, { "address": "23.100.87.56" }, { "address": "23.100.82.16" }], "accessEndpointIpAddresses": [{ "address": "13.67.236.76" }, { "address": "40.77.111.254" }, { "address": "40.77.31.87" }, { "address": "104.43.243.39" }] }, "connector": { "outgoingIpAddresses": [{ "address": "13.89.171.80/28" }, { "address": "40.122.49.51" }, { "address": "52.173.245.164" }] } } }, "id": "/subscriptions/af54c7f1-c5b0-4612-b395-336cae931fb0/resourceGroups/Dev/providers/Microsoft.Logic/workflows/Test", "name": "Test", "type": "Microsoft.Logic/workflows", "location": "centralus", "tags": {} }} />
                </div>
            );
        } else {
            return (
                <div  >
                    <WorkflowList />
                    <br />
                </div>
            );
        }

    }
}

class FlowDiagarm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            jsonTxt: '',
        };
        this.changeHandler = this.jsonChangeHandler.bind(this);
    }

    componentDidUpdate() {
        propArray = [];
        edgeArray = [];
        firstRes = true;
        cy.elements().remove();
        if (this.props.jsonTxt != null && this.props.jsonTxt != "") {
            jsonPass = this.props.jsonTxt;
            console.log(this.props.jsonTxt);
            this.renderJson(this.props.jsonTxt);
        }
    }

    componentDidMount() {
        var tt = document.getElementById('cy');
        if (tt != null) {
            cy = cytoscape({
                container: document.getElementById('cy'),
                style: [
                    {
                        selector: 'edge',
                        style: {
                            'curve-style': 'bezier',//taxi 
                            'target-arrow-shape': 'triangle',
                        }
                    },
                    {
                        selector: 'node',
                        style: {
                            'shape': 'rectangle',
                            'background-color': '#CCCCCC',
                            'border-width': 1,
                            'width': 80,
                            'text-valign': 'center',
                            'text-halign': 'center',
                            'background-blacken': 0,
                        }
                    }
                ],
                layout: {
                    name: 'grid',
                    rows: 10,
                }
            });
            cy.on('tap', 'node', function (evt) {
                var node = evt.originalEvent.target;
                if (node.localName == "input" || node.contentEditable) {
                    getFocus(node);
                }
            });
            cy.nodeHtmlLabel([{
                query: '.l1',
                valignBox: "center",
                halignBox: "center",
                cssClass: 'input',
                tpl: function (data) {
                    var retu = data.label;
                    cy.resize();
                    return retu;
                }
            },
            {
                query: first,
                tpl: function (data) {
                    var retu = data.label;
                    if (firstRes) {
                        resizeFlow(350);
                    }
                    if (this._node.children[0] != null) {
                        retu = this._node.children[0].outerHTML;
                    }
                    for (var i = 0; i < propArray.length; i++) {
                        var aux = document.getElementById('txt' + propArray[i][0] + '_' + propArray[i][1]);
                        if (aux != null && aux.defaultValue != null && data.id == propArray[i][0]) {
                            var auxtest = replaceAll(aux.value);
                            aux.defaultValue = auxtest;
                        }
                        if (this._node.children[0] != null) {
                            retu = this._node.children[0].outerHTML;
                        }
                    }
                    if (retu != "") data.label = retu;
                    return retu;
                }
            }
            ]);
        }
    }

    //Update Json
    jsonChangeHandler() {
        var id1 = document.getElementById('loaderId');
        var id2 = document.getElementById('cyId');
        var id3 = document.getElementById('btn_save');
        id1.removeAttribute("hidden");
        id2.setAttribute("hidden", true);
        id3.setAttribute("disabled", "disabled");
        var text = this.generateJson(jsonPass);
        if (text == '') {
            swal({
                title: "Error!",
                text: "Invalid Email!",
                icon: "error",
                button: "Ok",
            });
            var id2 = document.getElementById('loaderId');
            var id1 = document.getElementById('cyId');
            id1.removeAttribute("hidden");
            id2.setAttribute("hidden", true);
            id3.removeAttribute("disabled");
        } else {
            this.props.UpdateJsonAlert(text);
        }
    }

    //Generate the label that is shown inside the node, and render the nodes
    renderJson(json) {
        if (cy != null) {
            cy.elements().remove();
            propArray = new Array();
            edgeArray = new Array();
            var jsn = json.properties.definition.actions;
            var trigger = json.properties.definition.triggers;
            var lb_trg;
            var triggerN = "";
            for (var trg in trigger) {
                triggerN = trg;
                var tr = document.getElementById(trg);
                if (tr == null) {
                    lb_trg = "<div id=" + trg + ">Type: " + trigger[trg]['type'] + "<br/> Kind: " + trigger[trg]['kind'] + "</div>";
                    this.addNodeToCy('', 0, lb_trg, trg, trg);
                }
            }
            var auxy = this.setOrderNode(jsn);
            var htmlNodes = this.renderCy(jsn, '', '', auxy, true);
            //Order array
            htmlNodes = this.orderArray(htmlNodes, '');
            var arrayLeft = edgeArray;
            var arrayFirst = this.setOrderNode(jsn);
            var firstLength = 0;
            while (arrayLeft.length > 0) {
                var arrayRet = new Array();
                for (var i = 0; i < arrayLeft.length; i++) {
                    var type = 3;
                    if (arrayLeft[i][1] == 'first_edge') {
                        for (var j = 0; j < htmlNodes.length; j++) {
                            if (htmlNodes[j][0] == arrayLeft[i][0]) {
                                type = 4;
                                this.addNodeToCy(triggerN, type, htmlNodes[j][1], htmlNodes[j][0], htmlNodes[j][0]);
                            }
                            break;
                        }
                    } else {
                        var element = cy.getElementById(arrayLeft[i][1]).position();
                        if (typeof element != "undefined") {
                            var cantType = 0;
                            for (let k = 0; k < arrayFirst.length; k++) {
                                if (arrayFirst[k] == arrayLeft[i][0]) {
                                    for (let n = 0; n < arrayLeft.length; n++) {
                                        if (arrayLeft[n][1] == arrayLeft[i][1] && arrayFirst.includes(arrayLeft[n][0])) cantType++;
                                    }
                                }
                            }
                            for (var j = 0; j < htmlNodes.length; j++) {
                                if (htmlNodes[j][0] == arrayLeft[i][0]) {
                                    if (arrayFirst.includes(arrayLeft[i][0])) {
                                        if (htmlNodes[j][4] == 'else') {
                                            type = 1;
                                        }
                                        if (htmlNodes[j][4] == 'actions') {
                                            type = 2;
                                        }
                                        if (cantType > 1) {
                                            type = 5 + cantType;
                                        }
                                        var elementProp = document.getElementById(htmlNodes[j][0]);
                                        if (elementProp==null)this.addNodeToCy(arrayLeft[i][1], type, htmlNodes[j][1], htmlNodes[j][0], htmlNodes[j][0]);
                                        firstLength++;
                                        break;
                                    } else {
                                        if (firstLength + 1 < arrayFirst.length) {
                                            var pos = arrayRet.length;
                                            arrayRet[pos] = new Array();
                                            arrayRet[pos][0] = arrayLeft[i][0];
                                            arrayRet[pos][1] = arrayLeft[i][1];
                                        } else {
                                            if (htmlNodes[j][4] == 'else') {
                                                type = 1;
                                            }
                                            if (htmlNodes[j][4] == 'actions') {
                                                type = 2;
                                            }
                                            var elementProp = document.getElementById(htmlNodes[j][0]);
                                            if (elementProp == null)this.addNodeToCy(arrayLeft[i][1], type, htmlNodes[j][1], htmlNodes[j][0], htmlNodes[j][0]);
                                            break;
                                        }
                                    }
                                }
                            }
                        } else {
                            var pos = arrayRet.length;
                            arrayRet[pos] = new Array();
                            arrayRet[pos][0] = arrayLeft[i][0];
                            arrayRet[pos][1] = arrayLeft[i][1];
                        }

                    }
                }
                arrayLeft = arrayRet;
            }
            for (var i = 0; i < edgeArray.length; i++) {
                var get = cy.getElementById(edgeArray[i][0] + '_' + edgeArray[i][1]);
                var trg = cy.getElementById("edge_" + triggerN + "_" + edgeArray[i][0]);
                if (trg.length == 0 && triggerN != "" && edgeArray[i][1] == 'first_edge') {
                    this.addEdgeToCy("edge_" + triggerN + "_" + edgeArray[i][0], triggerN, edgeArray[i][0], '#000000'/*'#FF0000'*/);
                }
                if (edgeArray[i][1] != null && get.length == 0 && edgeArray[i][1] != 'first_edge') {
                    var ppl = this.setOrderNode(jsn);
                    var color = '#007bff';
                    for (let j = 0; j < ppl.length; j++) {
                        if (ppl[j] == edgeArray[i][0]) {
                            color = '#000000';
                        }
                    }
                    var elem = document.getElementById(edgeArray[i][0]);
                    if (elem.innerHTML.includes('True:')) {
                        color = '#7fba00';
                    }
                    if (elem.innerHTML.includes('False:')) {
                        color = '#FF0000';
                    }
                    this.addEdgeToCy(edgeArray[i][0] + '_' + edgeArray[i][1], edgeArray[i][1], edgeArray[i][0], color/*'#FF0000'*/);
                }
            }
        }
    }

    //Order the next array of property
    setOrderNode(jsn) {
        var arrayRet = new Array();
        var arrayKey = Object.keys(jsn);
        var first = true;
        while (arrayKey.length > 0) {
            var arrayAux = new Array();
            for (var j = 0; j < arrayKey.length; j++) {
                if ((typeof Object.keys(jsn[arrayKey[j]].runAfter)[0] === "undefined" || Object.keys(jsn[arrayKey[j]].runAfter)[0] == '') && first) {
                    var pos = arrayRet.length;
                    arrayRet[pos] = arrayKey[j];
                    first = false;
                } else {
                    if (first) {
                        var pos = arrayAux.length;
                        arrayAux[pos] = arrayKey[j];
                    } else {
                        var hs = false;
                        for (var i = 0; i < arrayRet.length; i++) {
                            if (arrayRet[i] == Object.keys(jsn[arrayKey[j]].runAfter)[0]) {
                                hs = true;
                            }
                        }
                        if (hs) {
                            var pos = arrayRet.length;
                            arrayRet[pos] = arrayKey[j];
                        } else {
                            var pos = arrayAux.length;
                            arrayAux[pos] = arrayKey[j];
                        }
                    }
                }
            }
            arrayKey = arrayAux;
        }
        return arrayRet;
    }

    //generate the new json on save
    generateJson(text) {
        var txt_json = text;
        var email = true;
        for (let k = 0; k < propArray.length; k++) {
            var aux = document.getElementById('txt' + propArray[k][0] + '_' + propArray[k][1]);

            if (aux != null) {
                if (propArray[k][1] == 'To') {
                    email = validateEmail(aux.value);
                }
                if (email) {
                    if (this.checkPermission(propArray[k][1])) {
                        var test = new Array();
                        test = this.getPaths(txt_json.properties.definition.actions, propArray[k], test, aux.value);
                    }
                } else {
                    return '';
                }
            }
        }
        return txt_json;
    }

    //jsonRes is the actual json, prop is the property for search, and pathArray is the path of the json
    //to the prop
    getPaths(jsonRes, prop, path, value) {
        for (let p in jsonRes) {
            if (p != "runAfter") {
                if (p == prop[0]) {
                    var pos = path.length;
                    path[pos] = p;
                    this.setValueJson(jsonRes[p], prop, value);
                    return path;
                } else {
                    if (jsonRes[p].hasOwnProperty(prop[0])) {
                        var pos = path.length;
                        path[pos] = p;
                        this.setValueJson(jsonRes[p][prop[0]], prop, value);
                        return path;
                    } else if (jsonRes[p] instanceof Object && this.getPaths(jsonRes[p], prop, path, value)) {
                        if (path == null) {
                            path = new Array();
                        } else {
                            return path;
                        }
                    }
                }
            }
        }
        return null;
    }

    //set the new values to the json, obj is the json object, path is the path to get the property
    //value is the new value to store, expression is a boolean for the expression property, and pp 
    //is the property name for expression
    setValueJson(json, prop, value) {
        console.log(prop);
        if (prop[2] == 'inputs') {
            json.inputs[prop[1]] = value;
        } else if (prop[2] == 'email') {
            json.inputs.body[prop[1]] = value;
        } else if (prop[2] == 'expression') {
            for (let j in json[prop[2]]) {
                for (let k in json[prop[2]][j]) {
                    for (let t in json[prop[2]][j][k]) {
                        if (t == [prop[1]]) json[prop[2]][j][k][t][1] = value;
                    }
                }
            }
        }
    }

    //order the htmlNodes array
    orderArray(htmlNodes, next) {
        var order = new Array();
        for (var i = 0; i < htmlNodes.length; i++) {
            if (next == '') {
                for (var j = 0; j < htmlNodes.length; j++) {
                    if (htmlNodes[j][2]) {
                        next = htmlNodes[j][0];
                        let aux = htmlNodes[i];
                        htmlNodes[i] = htmlNodes[j];
                        htmlNodes[j] = aux;
                        var pos = order.length;
                        order[pos] = next;
                        break;
                    }
                }
            } else {
                for (var j = 0; j < htmlNodes.length; j++) {
                    if (!htmlNodes[j][2]) {
                        for (var k = 0; k < edgeArray.length; k++) {
                            if (htmlNodes[j][0] == edgeArray[k][0] && edgeArray[k][1] == next) {
                                next = htmlNodes[k][0];
                                var pos = order.length;
                                order[pos] = next;
                                let aux = htmlNodes[j];
                                htmlNodes[j] = htmlNodes[k];
                                htmlNodes[k] = aux;
                                break;
                            }
                        }
                    }
                }
            }
        }
        return htmlNodes;
    }

    //generate the lbNode array
    renderCy(jsn, nameFrom, nameMap, auxy, edge) {
        var lbNode = new Array();
        for (var i = 0; i < auxy.length; i++) {
            var get = document.getElementById(auxy[i]);
            if (get == null) {
                for (var type in jsn[auxy[i]]) {
                    switch (type) {
                        case 'inputs':
                            var indice = 1;
                            var resType = '';
                            if (nameFrom == 'else') resType = 'False: ';
                            if (nameFrom == 'actions') resType = 'True: ';
                            var lb_msg = "<div id=" + auxy[i] + " style='width:100px;'> <div style='border-bottom: 1px solid black;text-align: center;width: 110px;margin-left: -5px;margin-bottom: 5px;'><b style='font-size: 6px;'>" + resType + " " + auxy[i] + "</b></div>";
                            var poss = edgeArray.length;
                            var firstNode = false;
                            if ((typeof Object.keys(jsn[auxy[i]].runAfter)[0] === "undefined" || Object.keys(jsn[auxy[i]].runAfter)[0] == '') && edge) {
                                edgeArray[poss] = new Array(2);
                                edgeArray[poss][0] = auxy[i];
                                edgeArray[poss][1] = 'first_edge';
                                firstNode = true;
                                edge = false;
                            } else {
                                for (var name in jsn[auxy[i]].runAfter) {
                                    edgeArray[poss] = new Array(2);
                                    edgeArray[poss][0] = auxy[i];
                                    edgeArray[poss][1] = name;
                                    poss = edgeArray.length;
                                }
                            }
                            var renderBody = this.renderAction(jsn[auxy[i]].inputs.body, 'Body');
                            if (renderBody != null) {
                                for (var propN in jsn[auxy[i]].inputs.body) {
                                    poss = propArray.length;
                                    propArray[poss] = new Array(3);
                                    propArray[poss][0] = auxy[i];
                                    propArray[poss][1] = propN;
                                    propArray[poss][2] = 'email';
                                    if (this.checkPermission(propN)) {
                                        lb_msg += "<span >" + propN.toUpperCase() + ": </span><textarea class='input' style='width:100%;resize: none;overflow: hidden;' type='text' id='txt" + auxy[i] + "_" + propN + "' >" + jsn[auxy[i]].inputs.body[propN] + "</textarea>";
                                    } else {
                                        lb_msg += "" + propN.toUpperCase() + ": <span style='width:100%' id='txt" + auxy[i] + "_" + propN + "'> " + jsn[auxy[i]].inputs.body[propN] + "</span><br />";
                                    }
                                    indice++;
                                }
                                lb_msg += "</div>";
                                var template = document.createElement('template');
                                template.innerHTML = lb_msg;
                                var nodePoss = lbNode.length;
                                lbNode[nodePoss] = new Array(5);
                                lbNode[nodePoss][0] = auxy[i];
                                lbNode[nodePoss][1] = lb_msg;
                                lbNode[nodePoss][2] = firstNode;
                                lbNode[nodePoss][3] = indice;
                                lbNode[nodePoss][4] = nameFrom;
                            } else {
                                for (var propN in jsn[auxy[i]].inputs) {
                                    if (propN == 'body') {
                                        poss = propArray.length;
                                        propArray[poss] = new Array(3);
                                        propArray[poss][0] = auxy[i];
                                        propArray[poss][1] = propN;
                                        propArray[poss][2] = 'inputs';
                                        if (this.checkPermission(propN)) {
                                            lb_msg += "<span >" + propN.toUpperCase() + ": </span><textarea class='input' style='width:100%;resize: none;overflow: hidden;' type='text' id='txt" + auxy[i] + "_" + propN + "' >" + jsn[auxy[i]].inputs[propN] + "</textarea>";
                                        } else {
                                            lb_msg += "" + propN.toUpperCase() + ": <span style='width:100%' id='txt" + auxy[i] + "_" + propN + "'> " + jsn[auxy[i]].inputs[propN] + "</span> <br>";
                                        }
                                        indice++;
                                    } else if (propN == 'headers') {
                                        lb_msg += "<span>" + propN.toUpperCase() + "</span><br>";
                                        for (let headers in jsn[auxy[i]].inputs[propN]) {
                                            poss = propArray.length;
                                            propArray[poss] = new Array(3);
                                            propArray[poss][0] = auxy[i];
                                            propArray[poss][1] = headers;
                                            propArray[poss][2] = 'expression';
                                            lb_msg += "<span style='width:20%'>" + headers.toUpperCase() + ":</span> <span style='width:80%' id='txt" + auxy[i] + "_" + headers + "'> " + jsn[auxy[i]].inputs[propN][headers] + "</span><br>";
                                        }
                                    } else {
                                        poss = propArray.length;
                                        propArray[poss] = new Array(3);
                                        propArray[poss][0] = auxy[i];
                                        propArray[poss][1] = propN;
                                        propArray[poss][2] = 'expression';
                                        lb_msg += "<span style='width:20%'>" + propN.toUpperCase() + ":</span> <span style='width:80%' id='txt" + auxy[i] + "_" + propN + "'> " + jsn[auxy[i]].inputs[propN] + "</span><br>";
                                    }
                                }
                                lb_msg += "</div>";
                                var template = document.createElement('template');
                                template.innerHTML = lb_msg;
                                var nodePoss = lbNode.length;
                                lbNode[nodePoss] = new Array(5);
                                lbNode[nodePoss][0] = auxy[i];
                                lbNode[nodePoss][1] = lb_msg;
                                lbNode[nodePoss][2] = firstNode;
                                lbNode[nodePoss][3] = indice;
                                lbNode[nodePoss][4] = nameFrom;
                            }
                            nameFrom = '';
                            nameMap = '';
                            break;

                        case 'actions':
                            for (var propN in jsn[auxy[i]]) {
                                var elementExist = document.getElementById(auxy[i]);
                                if (elementExist == null) {
                                    switch (propN) {
                                        case 'expression':
                                            var indice = 1;
                                            var lb_msg = "<div id=" + auxy[i] + " style='width:100px;'> <div style='border-bottom: 1px solid black;text-align: center;width: 110px;margin-left: -5px;margin-bottom: 5px;'><b style='font-size: 6px;'>" + auxy[i] + "</b></div>";
                                            var firstNode = false;
                                            if ((typeof Object.keys(jsn[auxy[i]].runAfter)[0] === "undefined" || Object.keys(jsn[auxy[i]].runAfter)[0] == '') && nameMap == '') {
                                                firstNode = true;
                                            }
                                            for (var propNJ in jsn[auxy[i]][propN]) {
                                                lb_msg += "" + propN.toUpperCase() + ": <span style='width:80%' id='txt" + auxy[i] + "_" + propN + "_" + propNJ + "'> " + propNJ.toUpperCase() + "</span><br />";
                                                for (var porpNJPE in jsn[auxy[i]][propN][propNJ]) {
                                                    for (var porpNJE in jsn[auxy[i]][propN][propNJ][porpNJPE]) {
                                                        poss = propArray.length;
                                                        propArray[poss] = new Array(3);
                                                        propArray[poss][0] = auxy[i];
                                                        propArray[poss][1] = porpNJE;
                                                        propArray[poss][2] = 'expression';
                                                        if (this.checkPermission(porpNJE)) {
                                                            lb_msg += "<span >" + jsn[auxy[i]][propN][propNJ][porpNJPE][porpNJE][0] + " " + porpNJE + ": </span><textarea class='input' style='width:100%;resize: none;overflow: hidden;' type='text' id='txt" + auxy[i] + "_" + porpNJE + "' >" + jsn[auxy[i]][propN][propNJ][porpNJPE][porpNJE][1] + "</textarea><br /> ";
                                                        } else {
                                                            lb_msg += "<span >" + jsn[auxy[i]][propN][propNJ][porpNJPE][porpNJE][0] + " " + porpNJE + ": </span> <span style='width:80%' id='txt" + auxy[i] + "_" + porpNJE + "'> " + jsn[auxy[i]][propN][propNJ][porpNJPE][porpNJE][1] + "</span><br />";
                                                        }
                                                        indice++;
                                                    }
                                                }
                                            }
                                            indice++;
                                            lb_msg += "" + "TYPE" + ": <span style='width:80%' id='txt" + auxy[i] + "_" + "type" + "'> " + jsn[auxy[i]]["type"] + "</span>";
                                            //if (!this.checkPermission('type')) {
                                            //    lb_msg += "<span >" + 'TYPE' + ": </span><textarea class='input' style='width:80%;resize: none;overflow: hidden;' type='text' id='txt" + auxy[i] + "_" + "type" + "' >" + jsn[auxy[i]]["type"] + "</textarea>";
                                            //} else {
                                                
                                            //}
                                            lb_msg += "</div>";
                                            var template = document.createElement('template');
                                            template.innerHTML = lb_msg;
                                            var nodePoss = lbNode.length;
                                            lbNode[nodePoss] = new Array(5);
                                            lbNode[nodePoss][0] = auxy[i];
                                            lbNode[nodePoss][1] = lb_msg;
                                            lbNode[nodePoss][2] = firstNode;
                                            lbNode[nodePoss][3] = indice;
                                            lbNode[nodePoss][4] = nameFrom;
                                            break;

                                        case 'actions':
                                            if (nameFrom == '') {
                                                nameFrom = 'actions';
                                                nameMap = auxy[i];
                                            }
                                            var nodeArr = this.setOrderNode(jsn[auxy[i]][propN]);
                                            var arrayActions = this.renderCy(jsn[auxy[i]][propN], nameFrom, nameMap, nodeArr, false);
                                            for (let i = 0; i < arrayActions.length; i++) {
                                                var nodePoss = lbNode.length;
                                                lbNode[nodePoss] = new Array(5);
                                                lbNode[nodePoss][0] = arrayActions[i][0];
                                                lbNode[nodePoss][1] = arrayActions[i][1];
                                                lbNode[nodePoss][2] = arrayActions[i][2];
                                                lbNode[nodePoss][3] = arrayActions[i][3];
                                                lbNode[nodePoss][4] = arrayActions[i][4];
                                                var exist = false;
                                                for (let j = 0; j < edgeArray.length; j++) {
                                                    if (edgeArray[j][0] == arrayActions[i][0]) {
                                                        exist = true;
                                                    }
                                                }
                                                if (!exist) {
                                                    var poss = edgeArray.length;
                                                    edgeArray[poss] = new Array(2);
                                                    edgeArray[poss][0] = arrayActions[i][0];
                                                    edgeArray[poss][1] = nameMap;
                                                }
                                            }
                                            nameFrom = '';
                                            nameMap = '';
                                            break;

                                        case 'else':
                                            nameFrom = 'else';
                                            nameMap = auxy[i];

                                            var nodeArr = this.setOrderNode(jsn[auxy[i]][propN].actions);
                                            var arrayActions = this.renderCy(jsn[auxy[i]][propN].actions, nameFrom, nameMap, nodeArr, false);
                                            for (let i = 0; i < arrayActions.length; i++) {
                                                var nodePoss = lbNode.length;
                                                lbNode[nodePoss] = new Array(5);
                                                lbNode[nodePoss][0] = arrayActions[i][0];
                                                lbNode[nodePoss][1] = arrayActions[i][1];
                                                lbNode[nodePoss][2] = arrayActions[i][2];
                                                lbNode[nodePoss][3] = arrayActions[i][3];
                                                lbNode[nodePoss][4] = arrayActions[i][4];
                                                var exist = false;
                                                for (let j = 0; j < edgeArray.length; j++) {
                                                    if (edgeArray[j][0] == arrayActions[i][0]) {
                                                        exist = true;
                                                    }
                                                }
                                                if (!exist) {
                                                    var poss = edgeArray.length;
                                                    edgeArray[poss] = new Array(2);
                                                    edgeArray[poss][0] = arrayActions[i][0];
                                                    edgeArray[poss][1] = nameMap;
                                                }
                                            }
                                            nameFrom = '';
                                            nameMap = '';
                                            break;

                                        case 'runAfter':
                                            var poss = edgeArray.length;
                                            if (typeof Object.keys(jsn[auxy[i]].runAfter)[0] === "undefined" || Object.keys(jsn[auxy[i]].runAfter)[0] == '' && edge) {
                                                edgeArray[poss] = new Array(2);
                                                edgeArray[poss][0] = auxy[i];
                                                edgeArray[poss][1] = 'first_edge';
                                                edge = false;
                                            } else {
                                                for (var name in jsn[auxy[i]].runAfter) {
                                                    var poss = edgeArray.length;
                                                    edgeArray[poss] = new Array(2);
                                                    edgeArray[poss][0] = auxy[i];
                                                    edgeArray[poss][1] = name;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        return lbNode;
    }

    //function to get if the user can edit the value
    checkPermission(propN) {
        var perm = true;
        for (var i = 0; i < permission.length; i++) {
            if (propN == permission[i]) {
                perm = false;
            }
        }
        if (permissionType != 5) {
            perm = false;
        }
        return perm;
    }

    //add the edges to the cy layout
    addEdgeToCy(id, source, target, color) {
        cy.add({
            group: 'edges',
            data: { id: id, source: source, target: target },
            style: {
                'line-color': color,
                'target-arrow-color': color,
            }
        });
    }

    //add the nodes to the cy layout
    addNodeToCy(previous, type, lb_msg, nameN, prop) {
        var x = 0;
        var y = 0;
        var tryF = true;
        if (type != 0) {
            var posCy = cy.getElementById(previous).position();
            x = posCy.x;
            y = posCy.y + 120;
            if (type == 1) {
                y -= 20;
                x += 150;
            } else if (type == 2) {
                y -= 20;
                x -= 150;
            } else if (type == 4) {
                y -= 10;
            } else if (type > 5) {
                x -= 120;
            }
            var valid = false;
            var cant = 0;
            while (!valid) {
                if (this.checkPossition(x, y)) {
                    valid = true;
                } else {
                    cant++;
                    if (x < 0 && type < 5) {
                        if (tryF) {
                            x -= 150;
                            tryF = false;
                        } else {
                            x += 150;
                            y -= 100;
                            tryF = true;
                        }
                    } else if (x > 0 && type < 5) {
                        if (tryF) {
                            x += 150;
                            tryF = false;
                        } else {
                            x -= 150;
                            y += 100;
                            tryF = true;
                        }
                    } else if (type == 2) {
                        y -= 200;
                        x += 300;
                    } else if (type == 1) {
                        y -= 200;
                        x -= 300;
                    } else if (type > 5) {
                        x += 240;
                    } else {
                        y += 120;
                    }
                }
                if (cant == 50) {
                    valid = true;
                }
            }
        }
        cy.add({
            group: 'nodes',
            data: { id: prop, name: nameN, label: lb_msg },
            position: { x: x, y: y },
            classes: 'l2',
            selector: 'node',
        });
    }

    //check if in the x and y position there is an node
    checkPossition(x, y) {
        var difx = 0;
        var dify = 0;
        for (let i = 0; i < edgeArray.length; i++) {
            var element = cy.getElementById(edgeArray[i][0]).position();
            if (typeof element != "undefined") {
                if (x > element.x) {
                    difx = x - element.x;
                } else {
                    difx = element.x - x;
                }
                if (y > element.y) {
                    dify = y - element.y;
                } else {
                    dify = element.y - y;
                }
                if (difx < 100 && dify < 100) {
                    return false;
                }
            }
        }
        return true;
    }


    //gets the property inside the json
    renderAction(jsonIn, action) {
        var jsonRes = jsonIn;
        for (var p in jsonIn) {
            if (jsonIn.hasOwnProperty(p)) {
                if (p === action) {
                    jsonRes = jsonIn;
                    return jsonRes;
                } else if (jsonIn[p] instanceof Object && this.renderAction(jsonIn[p], action)) {
                    return jsonIn[p];
                }
            }
        }
    }

    render(props) {
        return (
            <button id="btn_save" type="button" className="btn btn-primary" onClick={this.changeHandler.bind(this)} >Save changes</button>
        );
    }
}

ReactDOM.render(<App />, document.getElementById('root'));