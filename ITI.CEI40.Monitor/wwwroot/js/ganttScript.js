
var preRowsNo = 0;
var createdRowsIds = [];
var deletedDep = [];   // an array to store dependency objects to delete from DB
var removeFromDB = [];  // an array to store dbids to delete from DB
var tasksIdOrder = [0];
var tasks = [];
var filledRows = [];
var assigneeTeams = [];

// add function that will fire with splice
//function processQ() {
//    // ... this will be called on each .push
//    console.log("*");
//    console.log("");
//}

//var tasks = [];
//tasks.splice = function () { Array.prototype.push.apply(this, arguments); processQ(); };



class Id {
    constructor(id) {
        this.id = id
    }
}

class Dependency {
    constructor(depId, lag) {
        this.depId = depId;
        this.lag = lag;
    }
}

class Task {
    constructor(id, name, start, end, duration, assignee, progress) {
        this.id = id;
        this.name = name;
        this.start = start;
        this.end = end;
        this.duration = duration;
        this.depends;
        this.dependencies = [];
        this.progress = progress;
        this.assignee = assignee;
        this.dbId = "";
        this.status = "STATUS_UNDEFINED";

    }
}

var dep = new Dependency("Dependency0", 2, 2);

var trydata = [{ id: "0", name: "p1", depends: -1, start: new Date(), end: new Date(2020, 0, 1) }];


//document.onload = table();
function table(dataArray, noData) {


    if (noData) {
        if (!dataArray.length > 0) {
            createRowTask(1);
        } else {
            createRowTaskWithData(dataArray);
            createRowTask(1);
        }
        draw();
    }

}



// s is format y-m-d
// Returns a date object for 00:00:00 local time
// on the specified date
function parseDate(s) {
    var b = s.split(/\D/);
    return new Date(b[2], --b[1], b[0]);
}

function isHoliday(date) {
    var h_date = new Date(date);
    if (disableAllTheseDays(h_date) || h_date.getDay() == 5 || h_date.getDay() == 6) {
        return true;
    }
    return false;
}

function computeDuration(date1, date2) {
    date2.setHours(23, 59, 59, 999);
    var absDuration = Math.ceil((date2 - date1) / 86400000);
    var d = new Date(date1);
    var duration = 0;
    for (let i = 0; i < absDuration; i++) {
        if (!isHoliday(d)) {
            duration++;
        }
        d.setDate(d.getDate() + 1);
    }
    return duration;
}

function computeEndFromDuration(date1, Duration) {
    var d = new Date(date1);
    var duration = Duration.value - 1;
    while (duration > 0) {
        if (!isHoliday(d)) {
            duration--;
        }
        d.setDate(d.getDate() + 1);
    }
    return d;
}

function computeEndFromDurationVal(date1, Duration) {
    var d = new Date(date1);
    var duration = Duration - 1;
    while (duration > 0) {
        if (!isHoliday(d)) {
            duration--;
        }
        d.setDate(d.getDate() + 1);
    }
    return d;
}

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [day, month, year].join('/');
}

// the disabled days
var disabledDays = [];
function disableAllTheseDays(date) {
    stringdate = formatDate(date);
    for (let i = 0; i < disabledDays.length; i++) {
        if (disabledDays[i] === stringdate) {
            return true;
        }
    }
    return false;
}



// the body
function createRowTask(num) {
    num = num + preRowsNo;
    for (let n = preRowsNo; n < num; n++) {
        var tr = tableRow(n);
        $('#tbody').append(tr);
        createdRowsIds.push(parseInt(n));
        addAssignOpts(n);
    }
    preRowsNo = num;
}

function tableRow(n) {
    var datapt1 = `<td style="display: flex;" ><label>${n + 1}</label>
        <button id="del-${n}" onclick="deleteRow(this.parentElement.parentElement.id); draw();"><span class="ui-icon ui-icon-trash" style="display:inline-block"></span></button>
        <button onclick="addRow(this.parentElement.parentElement.id)"><span class="ui-icon ui-icon-arrowstop-1-s" style="display:inline-block"></span></button >
        </td>
        <td><input type="text" id="taskName${n}" class="Name test no-outline"></td>`;

    var td1 = $('<td></td>');
    var datepicker1 = $(`<input type="text" id="startDate${n}" class="datepicker no-outline" readonly>`);
    datepicker1.datepicker({
        disabled: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        firstDay: 0,   //---Set The First Day in the Week ( Saturday)
        beforeShowDay: function (dt) {
            return [disableAllTheseDays(dt) || dt.getDay() == 5 || dt.getDay() == 6 ? false : true];
        }, //----   (Disable The Week Ends in Datepicker  )
        //minDate: 0, //-----Disable the previous Days 
        onSelect: function () {
            var startDate = $(`#startDate${n}`).datepicker('getDate');
            var endDate = $(`#endDate${n}`).datepicker('getDate');
            var Duration = $(`#Duration${n}`)
            if (startDate && endDate) {
                Duration.val(computeDuration(startDate, endDate));
                // var selectedId = tasks.findIndex(x => x.id === `${n}`);
                // console.log(selectedId);
            }
            assignTask(parseInt(n));
            draw();
        }
    });
    td1.append(datepicker1);

    var td2 = $('<td></td>');
    var datepicker2 = $(`<input type="text"  id="endDate${n}" class="datepicker no-outline" readonly>`);
    datepicker2.datepicker({
        disabled: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        firstDay: 0,   //---Set The First Day in the Week ( Saturday)
        beforeShowDay: function (dt) {
            return [disableAllTheseDays(dt) || dt.getDay() == 5 || dt.getDay() == 6 ? false : true];
        }, //----   (Disable The Week Ends in Datepicker  )
        //minDate: 0, //-----Disable the previous Days 
        onSelect: function () {
            var startDate = $(`#startDate${n}`).datepicker('getDate');
            var endDate = $(`#endDate${n}`).datepicker('getDate');
            var Duration = $(`#Duration${n}`)
            if (startDate && endDate) {
                Duration.val(computeDuration(startDate, endDate));
            }
            var selectedTask = tasks.find(x => x.id.id === n);
            if (selectedTask.dependencies.length > 0) {
                var arr = tasksToUpdate(selectedTask); //selectedTask.dependencies;
                arr.forEach(function (x) {
                    //var idNum = tasks[x].id;
                    var targetTask = tasks.find(t => t.id.id == x.id);
                    var idNum = targetTask.id.id;
                    var oldEndDate = tasks.find(t => t.id.id == n);
                    var lag = computeLag(oldEndDate.end, endDate);
                    shiftTaskDate(targetTask.start, targetTask.duration, lag, idNum);
                    assignTask(idNum);
                });
            }
            assignTask(parseInt(n));
            draw();
        }
    });
    td2.append(datepicker2);

    var datapt2 = `<td><input type="text" id="Duration${n}" class="durations no-outline" readonly></td>
        <td><input type="text" id ="Dependency${n}" onchange="dependency(this)"
            class="no-outline" onkeyup="wirteDependency(this)" readonly>
        </td>`
    var assigneeList =
        `<td><input hidden list="assign" name="assignees" class="no-outline" id = "assign${n}" readonly />
            <select disabled id="assignlist${n}" style="background:transparent; border:none;"> 
            <option value="" >Select Department</option>
            </select >
        </td>`
    var progress =
        `<td hidden><input type="number" min="0" max="100" value="0" class="no-outline progress" id = "progress${n}" readonly></td>`;

    var tr = $(`<tr></tr>`)
    tr.attr("id", `tempT-${n}`);
    tr.append(datapt1);
    tr.append(td1);
    tr.append(td2);
    tr.append(datapt2);
    tr.append(assigneeList);
    tr.append(progress);

    return tr;
}

//options for assignees
function addAssignOpts(n) {
    var select = document.getElementById(`assignlist${n}`);
    assigneeTeams.forEach(function (ass) {
        var el = document.createElement("option");
        el.value = ass.depId;
        el.innerHTML = ass.depName;
        if (select) { select.appendChild(el); }
    });

}

// validation for progress
$('tbody').on('input', '.progress', function (e) {
    if (e.target.value > 100 || e.target.value < 0) {
        e.target.classList.add('errprogress');
    } else {
        e.target.classList.remove("errprogress");
    }

});

function tasksToUpdate(rootTask) {
    var arrayOfDeps = [];
    traverse(rootTask, arrayOfDeps);
    return arrayOfDeps;
}
function traverse(rootTask, arrayOfDeps) {
    if (rootTask.dependencies.length == 0) { return; }
    for (let i = 0; i < rootTask.dependencies.length; i++) {
        arrayOfDeps.push(rootTask.dependencies[i].depId);
        traverse(tasks.find(t => t.id.id == rootTask.dependencies[i].depId.id), arrayOfDeps);
    }
}



$('tbody').on('change', '.Name', function (e) {
    e.preventDefault();
    var rowid = e.target.id;
    var id = rowid.toString().split("taskName")[1];
    id = parseInt(id);
    var rowValue = e.target.value;
    if (rowValue) {
        openRow(id);
        fillRowTask(e);
        if (!filledRows.includes(parseInt(id))) {
            filledRows.push(parseInt(id));
            createRowTask(1);
        }
        // console.log(filledRows);
        // console.log(preTaskName);
    } else {
        var rowid = e.target.id;
        var id = rowid.toString().split("taskName")[1];
        var name = $(`#taskName${id}`)[0].value;
        var selectedId = tasks.findIndex(x => x.id === `${id}`);
        console.log(selectedId);
    }
});

$('tbody').on('change', function (e) {
    var row = e.target.parentElement.parentElement;
    var rowNumber = row.id.split('tempT-')[1];
    assignTask(parseInt(rowNumber));
    // open the submit button after changes
    openSubmission();
    draw();
});

function openSubmission() {
    document.getElementById("submitBtn").disabled = false;
}

function fillRowTask(e) {
    var id = e.target.id;
    var n = id.toString().split("taskName")[1];
    var date = new Date();
    var startDate = $(`#startDate${n}`)[0].value;
    var endDate = $(`#endDate${n}`)[0].value;
    var Duration = $(`#Duration${n}`)
    if (!startDate && !endDate) {
        $(`#startDate${n}`).datepicker("setDate", date);
        $(`#endDate${n}`).datepicker("setDate", date);
        var startDate = parseDate($(`#startDate${n}`)[0].value);
        var endDate = parseDate($(`#endDate${n}`)[0].value);
        Duration.val(computeDuration(startDate, endDate));
    }
    assignTask(parseInt(n));
}


$('tbody').on('change', '.durations', function (e) {
    var id = e.target.id;
    var duration = document.getElementById(id);
    var n = id.toString().split("Duration")[1];
    var startDate = document.getElementById(`startDate${n}`).value;
    startDate = parseDate(startDate);
    var endDate = document.getElementById(`endDate${n}`);
    var re = computeEndFromDuration(startDate, duration);
    endDate.value = formatDate(re);

});

function closeRows() {
    $('table tr').each(function () {
        $(this).find('td').each(function () {
            $(this).find('input,select,textarea').attr("readonly", true);
        });
    });

    $(".datepicker").each(function (i) {
        //console.log(i);
        $(this).datepicker({ disabled: true })
    });
}

function closeTaskAtrr() {
    $('table tr').each(function () {
        $(this).find('td').each(function () {
            $(this).find('input,select,textarea').attr("readonly", true);
        });
    });

    $(".datepicker").each(function (i) {
        //console.log(i);
        $(this).datepicker({ disabled: true })
    });

    $('.Name').each(function () {
        $(this).attr("readonly", false);
    })
}

function openRow(id) {
    $(`#tempT-${id}`).each(function () {
        $(this).find('td').each(function () {
            $(this).find('input,select,textarea').attr("readonly", false);
            $(this).find('input,select,textarea').attr("disabled", false);
        });
    });
    $(`#startDate${id}`).datepicker("option", "disabled", false);
    $(`#endDate${id}`).datepicker("option", "disabled", false);
}


// we have a check here
function deleteRow(rowid) {
    var id = rowid.toString().split('-')[1];
    //remove the dependency objects from the two ways
    removeDependency(parseInt(id));
    // delete row
    var row = document.getElementById(rowid);
    row.parentNode.removeChild(row);
    //check if there is a task before shifting
    if (tasks.find(t => t.id.id == parseInt(id))) {
        //remove it from filledRows
        filledRows.splice(parseInt(id), 1);
        // add the removed task to removedfromDB if it wasn't new
        var removed = tasks.find(t => t.id.id == parseInt(id)).dbId;
        if (removed && !removeFromDB.includes(removed)) {
            removeFromDB.push(removed);
        }
        //remove task
        tasks.splice(parseInt(id), 1);
    }

    // shifting the ids (The Table Side)
    var remainingElements = preRowsNo - (parseInt(id) + 1);
    for (let i = 0; i < remainingElements; i++) {
        var n = parseInt(id) + i + 1;
        var nextRow = document.getElementById(`tempT-${n}`);
        nextRow.id = `tempT-${n - 1}`;
        var label = nextRow.firstChild.firstChild;
        label.innerText = n;
        var name = document.getElementById(`taskName${n}`);
        name.id = `taskName${n - 1}`;
        var Start = document.getElementById(`startDate${n}`);
        Start.id = `startDate${n - 1}`;
        var end = document.getElementById(`endDate${n}`);
        end.id = `endDate${n - 1}`;
        var duration = document.getElementById(`Duration${n}`);
        duration.id = `Duration${n - 1}`;
        var dependency = document.getElementById(`Dependency${n}`);
        dependency.id = `Dependency${n - 1}`;
        var assign = document.getElementById(`assign${n}`);
        assign.id = `assign${n - 1}`;
        var assignlist = document.getElementById(`assignlist${n}`);
        assignlist.id = `assignlist${n - 1}`;
        var progress = document.getElementById(`progress${n}`);
        progress.id = `progress${n - 1}`;

        // shifting the tasks also
        var t = tasks.find(t => t.id.id == n);
        if (t) { t.id.id = parseInt(t.id.id) - 1; }

        //managing rows
        var idIndex = createdRowsIds.findIndex(i => i == parseInt(n));
        createdRowsIds.splice(idIndex, 1, (parseInt(n) - 1));
    }

    // remove it from createdRowsIds
    createdRowsIds.splice(parseInt(id), 1);

    // update the value of the dependency
    updateDependency();
    // decrease from the total number
    preRowsNo--;
    // redraw the tasks
    draw();
    // open the submit button after changes
    openSubmission();
}


function removeDependency(id) {
    var task = tasks.find(t => t.id.id == id);

    if (task && task.depends) {
        var independentTaskId = task.depends.depId.id;
        var independentTask = tasks.find(t => t.id.id == independentTaskId);
        // save the deleted dependencies 
        if (!deletedDep.find(d => d.following == task.dbId)) {
            var deletedDependency = { followed: independentTask.dbId, following: task.dbId };
            deletedDep.push(deletedDependency);
        }
        var depTaskdep = independentTask.dependencies.findIndex(d => d.depId.id == task.id.id);
        //remove the dependency from the independent task
        independentTask.dependencies.splice(depTaskdep, 1);
    }
    if (task && task.dependencies.length > 0) {
        // remove the dependencies relations
        task.dependencies.forEach(function (i) {
            var t = tasks.find(t => t.id.id == i.depId.id);
            // save the deleted dependencies 
            if (t.depends) {
                if (!deletedDep.find(d => d.followed == task.dbId)) {
                    var deletedDependency = { followed: task.dbId, following: t.dbId };
                    deletedDep.push(deletedDependency);
                }
            }
            t.depends = null;
            document.getElementById(`Dependency${t.id.id}`).value = "";
            $(`#startDate${t.id.id}`).datepicker("option", "disabled", false);
        })
    }
}

function DeletedDependency(t) {
    // save the deleted dependencies 
    if (t.depends) {
        var DBid = tasks.find(d => d.dbId == t.dbId);

        if (!deletedDep.find(d => d.followed == t.depends.depId.id)) {
            var followdId = tasks.find(ta => ta.depends.depId.id == t.depends.depId.id).dbId;
            var followed = tasks.find(ta => ta.dbId == t.dbId);
            var deletedDependency = { followed: t.depends.depId.id, following: t.id.id };
            deletedDep.push(deletedDependency);
        }
    }
}

function addRow(rowid) {
    var id = parseInt(rowid.toString().split('-')[1]);

    // shifting the ids 
    var remainingElements = preRowsNo - (parseInt(id) + 1);
    for (let i = remainingElements; i > 0; i--) {
        // shifiting the rows themselves 
        var n = i + id;
        var nextRow = document.getElementById(`tempT-${n}`);
        nextRow.id = `tempT-${n + 1}`;
        var label = nextRow.firstChild.firstChild;
        label.innerText = n + 2;
        var name = document.getElementById(`taskName${n}`);
        name.id = `taskName${n + 1}`;
        var Start = document.getElementById(`startDate${n}`);
        Start.id = `startDate${n + 1}`;
        var end = document.getElementById(`endDate${n}`);
        end.id = `endDate${n + 1}`;
        var duration = document.getElementById(`Duration${n}`);
        duration.id = `Duration${n + 1}`;
        var dependency = document.getElementById(`Dependency${n}`);
        dependency.id = `Dependency${n + 1}`;
        var assign = document.getElementById(`assign${n}`);
        assign.id = `assign${n + 1}`;
        var assignlist = document.getElementById(`assignlist${n}`);
        assignlist.id = `assignlist${n + 1}`;
        var progress = document.getElementById(`progress${n}`);
        progress.id = `progress${n + 1}`;

        // shifting the tasks also
        var t = tasks.find(t => t.id.id == n);
        if (t) { t.id.id = parseInt(t.id.id) + 1; }
        //managing rows
        var idIndex = createdRowsIds.findIndex(i => i == parseInt(n));
        createdRowsIds.splice(idIndex, 0, (parseInt(n) + 1));
    }
    var tr = tableRow(id + 1);
    //var trr = $('table > tbody > tr').eq(id).after(tr);
    tr.insertAfter($(`#tempT-${id}`));
    createdRowsIds.push(parseInt(id));
    // update the value of the dependency
    updateDependency();
    addAssignOpts(n);
    preRowsNo++;
    // check if the row has data or delete it 
    checkFillingRow(id);
}

function checkFillingRow(id) {
    id = parseInt(id) + 1;
    $(`#taskName${id}`).focus();
    $(`#taskName${id}`).blur(function () {
        var val = $(`#taskName${id}`).val();
        if (val == null || val == "" || val == undefined) {
            deleteRow(`tempT-${id}`);
        }
    })
}

function updateDependency() {
    tasks.forEach(function (t) {
        if (t.depends) {
            $(`#Dependency${t.id.id}`).val(t.depends.depId.id + 1);
        }
    })
}

// attached to OnChange event 
//complete one senario
function dependency(cell) {
    var cellId = cell.id;
    var dep = cell.value;
    var depTaskId = parseInt(cellId.toString().split("Dependency")[1]);
    var deps = dep.toString().split(':');
    var targetLag = parseInt(deps[1]);
    //repeated
    var independentTask = tasks.find(x => x.id.id === depTaskId - 1);
    // var index = dependentTask.depends.depId;
    // var independentTask = tasks.find(x => x.id.id === index.id);
    if (dep) {
        if (parseInt(deps[0]) == (parseInt(depTaskId) + 1)) {
            alert("closed loop in dependecy");
            cell.value = "";
            return;
        }
        if (deps[0] <= 0 || !createdRowsIds.includes(parseInt(deps[0])) || !independentTask) {
            alert("no tasks with this number");
            cell.value = "";
            return;
        }
        var depIdObj = tasks.find(t => t.id.id == (deps[0] - 1)).id;
        var indepIdObj = tasks.find(t => t.id.id == depTaskId).id;
        var dependecy = new Dependency(depIdObj, targetLag);
        var inDependecy = new Dependency(indepIdObj, targetLag);
        var targetEndDate = parseDate($(`#endDate${deps[0] - 1}`)[0].value);
        var startDate = parseDate($(`#startDate${depTaskId}`)[0].value);
        //var endDate = parseDate($(`#endDate${n}`)[0].value);
        var preDurat = $(`#Duration${depTaskId}`)[0];
        var lag = computeLag(startDate, targetEndDate);
        //console.log("lag = " + lag);
        if (lag < 0) {
            if (targetLag) { lag = lag - targetLag - 1; }
            shiftTaskDate(startDate, preDurat.value, lag, depTaskId);
            lag = 0; // this is now
        }
        if (lag > 0 && targetLag) { lag = lag + targetLag; }
        dependecy.lag = lag;
        inDependecy.lag = lag;

        $(`#startDate${depTaskId}`).datepicker("option", "disabled", true);
        var dependentTask = tasks.find(x => x.id.id === depTaskId);
        var independentTask = tasks.find(x => x.id.id === deps[0] - 1);
        dependentTask.depends = dependecy; // `${deps[0]}`;
        if (!independentTask.dependencies.includes(inDependecy)) { independentTask.dependencies.push(inDependecy); }
    } else {
        var depTaskId = cell.id.toString().split("Dependency")[1];
        var inputType = typeof preValue;
        var preValue = cell.value;
        if (preValue == "") { inputType = typeof parseInt(preValue); }
        if (cell.value == "" && inputType == 'number') {
            $(`#startDate${depTaskId}`).datepicker("option", "disabled", false);
            var dependentTask = tasks.find(x => x.id.id === parseInt(depTaskId));

            var index = dependentTask.depends.depId;
            var independentTask = tasks.find(x => x.id.id === index.id);

            var inDependecy = new Dependency(dependentTask.id, dependentTask.depends.lag);
            //--- delete dependencies from the two ways ---//
            // save the deleted dependencies 
            var deletedDependency = { followed: independentTask.id.id, following: dependentTask.id.id };
            if (!deletedDep.find(d => d.followed == independentTask.id.id)) {
                deletedDep.push(deletedDependency);
            }

            dependentTask.depends = null; // delete 
            var depIndex = independentTask.dependencies.findIndex(t => t.depId.id == inDependecy.depId.id);
            if (independentTask.dependencies.some(d => d.depId.id == inDependecy.depId.id)) { independentTask.dependencies.splice(depIndex, 1); }
        }
    }

}

function wirteDependency(input) {
    var regex = /[^0-9:]/gi;
    input.value = input.value.replace(regex, "");
}

function shiftTaskDate(startDate, preDurat, lag, n) {
    var d1 = new Date(startDate);
    //var d2 = new Date(endDate);
    if (lag < 0) {
        while (lag < 0) {
            if (!isHoliday(d1)) {
                lag++;
            }
            d1.setDate(d1.getDate() + 1);
            //d2.setDate(d2.getDate() + 1);
        }
        while (isHoliday(d1)) { d1.setDate(d1.getDate() + 1); } //in case of stopping at thursday
    }

    if (lag > 0) {
        while (lag > 0) {
            if (!isHoliday(d1)) {
                lag--;
            }
            d1.setDate(d1.getDate() - 1);
        }
        while (isHoliday(d1)) { d1.setDate(d1.getDate() + 1); } //in case of stopping at thursday
    }
    var SDate = document.getElementById(`startDate${n}`);
    SDate.value = formatDate(d1);
    var EDate = document.getElementById(`endDate${n}`);
    var re = computeEndFromDurationVal(d1, preDurat);
    while (isHoliday(re)) { re.setDate(re.getDate() + 1); }
    EDate.value = formatDate(re);
}

function computeLag(startDate, targetEndDate) {
    var lag = Math.ceil((startDate - targetEndDate) / 86400000);

    var sDay = new Date(startDate);
    while (sDay.getDate() != targetEndDate.getDate() && lag < 0) {
        sDay.setDate(sDay.getDate() + 1);
        if (isHoliday(sDay)) { lag++; }
    } // remove holidays from the abslute lag when lag <0
    while (sDay.getDate() != targetEndDate.getDate() && lag > 0) {
        sDay.setDate(sDay.getDate() - 1);
        if (isHoliday(sDay)) { lag--; }
    } // remove holidays from the abslute lag when lag <0
    return lag;
}

/* #region  task asigning */
function assignTask(n) {
    var taskId = n;
    if (!tasks.find(t => t.id.id == taskId)) {
        var task = createTaskObj(taskId);
        tasks.splice(task.id.id, 0, task);
    } else {
        replaceTask(taskId);
    }
}

function createTaskObj(n) {
    var taskId = new Id(parseInt(n));
    var taskName = $(`#taskName${n}`)[0].value;
    var Sdate = $(`#startDate${n}`)[0].value;
    Sdate = parseDate(Sdate);
    var Edate = $(`#endDate${n}`)[0].value;
    Edate = parseDate(Edate);
    var Duration = $(`#Duration${n}`)[0].value;
    var assignee = $(`#assignlist${n}`)[0].value;

    var task = new Task(taskId, taskName, Sdate, Edate, Duration, assignee);
    task.tableViewOrder = n;
    return task;
}

function replaceTask(n) {
    var newTask = createTaskObj(n);
    var index = tasks.findIndex(t => t.id.id == n);
    if (index >= 0) {
        var depArr = tasks.find(t => t.id.id == index).dependencies;
        newTask.dependencies = depArr;
        var oldDep = tasks[index].depends;
        newTask.depends = oldDep;
        newTask.id = tasks[index].id;  // to save the reference obj
        newTask.dbId = tasks[index].dbId; // save the same DB id when update 
        tasks.splice(index, 1, newTask);
    }
    //console.log(tasks);
}

function assignTasks() {
    var dataToDB = [];
    tasks.forEach(function (t) {
        var viewmodelTask = {
            id: t.id.id,
            name: t.name,
            start: t.start.getTime(),
            end: t.end.getTime(),
            duration: t.duration,
            progress: t.progress,
            dbId: t.dbId,
            assignee: t.assignee
        }
        if (t.depends != -1 && t.depends != -2 && t.depends != null) {
            viewmodelTask.Dependecy = { id: t.depends.depId.id, lag: t.depends.lag };
        }
        if (t.dependencies.length > 0 && t) {
            viewmodelTask.Dependecies = convertDependencies(t);
        }

        dataToDB.push(viewmodelTask);
    });
    return dataToDB;
}

function convertDependencies(t) {
    var Dependecies = [];
    t.dependencies.forEach(function (d) {
        Dependecies.push({ id: d.depId.id, lag: d.lag })
    });
    return Dependecies;
}
/* #endregion */


/* #region  Frapp Chart */

/* {
    id: 'Task 1',
    name: 'Redesign website',
    start: '2020-12-31',
    end: '2021-12-31',
    progress: 20,
    dependencies: 'Task 1',
    custom_class: 'critical'
}*/
function formatGanttDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}

var ganttTasks = [];


var oldS = "";
var oldE = "";
var options = {
    header_height: 36,
    column_width: 30,
    step: 40,
    view_modes: ['Day', 'Week', 'Month'],
    bar_height: 21,
    bar_corner_radius: 3,
    arrow_curve: 5,
    padding: 18,
    view_mode: 'Day',
    date_format: 'DD/MM/YYYY',
    custom_popup_html: null,

    on_click: function (task) {
        //console.log(task);
    },
    on_date_change: function (task, start, end) {
        changeDates(task, start, end);
    },

    on_progress_change: function (task, progress) {
        //console.log(task, progress);
    },
    on_view_change: function (mode) {
        //console.log(mode);
    }
};

function draw() {
    ganttTasks = [];
    tasks.forEach(function (t) {
        var ganttTask = {
            id: `${t.id.id}`,
            name: `${t.name}`,
            start: `${formatGanttDate(t.start)}`,
            end: `${formatGanttDate(t.end)}`,
            progress: '20',
            dependencies: `${t.depends ? t.depends.depId.id : ""}`
        };
        ganttTasks.push(ganttTask);
    });
    if (ganttTasks.length > 0) { ganttChart(); }
}
function ganttChart() {
    var gantt = new Gantt('#ganttchart', ganttTasks, options);
    //var gantt = new Gantt("#gantt", data);
    var new_height = gantt.$svg.getAttribute('height') - 100;
    gantt.$svg.setAttribute('height', new_height);
}

function changeViewMode(vm) {
    if (vm == 1) {
        options.view_mode = "Day";
        //vm++;
    } else if (vm == 2) {
        options.view_mode = "Week";
        //vm++;
    } else {
        options.view_mode = "Month";
    }
    ganttChart();
}
// because of the arrangement of date elements
function parseGanttDate(s) {
    var b = s.split(/\D/);
    return new Date(b[0], --b[1], b[2]);
}
function changeDates(frappTask, frappStart, frappEnd) {
    var taskid = parseInt(frappTask.id);
    $(`#startDate${taskid}`)[0].value = formatDate(frappStart);
    $(`#endDate${taskid}`)[0].value = formatDate(frappEnd);
    $(`#Duration${taskid}`)[0].value = computeDuration(frappStart, frappEnd);

    assignTask(taskid);
    ganttTasks.find(t => t.id == taskid).start = formatGanttDate(frappStart);
    ganttTasks.find(t => t.id == taskid).end = formatGanttDate(frappEnd);
}

/* #endregion */

// send the array of tasks to the backend
function submit() {
    var proId = $('#proId').val();
    var Acts = assignTasks();

    $.ajax({
        type: "POST",
        url: "/Activity/AddActivities",
        data: { id: proId, Acts: Acts, reDbId: removeFromDB, reDep: deletedDep },
        success: function (response) {
            //console.log("fol: " + response);
            alert("Project tasks updated successfully");
            if (alert) {
                window.location.reload();
            }
        },
        error: function (x, y, err) {
            console.log(arguments);
        }
    });
}

// split view handling
var parent = document.querySelector('.splitview'),
    tablePanel = parent.querySelector('.tablecontent'),
    ganttPanel = parent.querySelector('.ganttcontent'),
    handle = parent.querySelector('.handle');
document.addEventListener('DOMContentLoaded', function () {

    var flag = false; var once = false;
    document.addEventListener("mousedown", function (event) { flag = true; once = true; });
    document.addEventListener("mouseup", function (event) {
        flag = false; once = false;
        tablePanel.style.pointerEvents = "auto";
        ganttPanel.style.pointerEvents = "auto";
    });
    parent.addEventListener("mousemove", function (event) {
        if (event.target.className === "handle" && once) { once = false; }
        if (flag && !once) {
            tablePanel.style.pointerEvents = "none";
            ganttPanel.style.pointerEvents = "none";
            // Move the handle.
            handle.style.left = event.clientX + 'px';
            // Adjust the table panel width.
            tablePanel.style.width = event.clientX + 'px';
            // Adjust the gantt panel width.
            ganttPanel.style.left = event.clientX + 'px';
        }

    });

});
function spliterTo(ratio) {
    // Move the handle.
    handle.style.left = ratio + '%';
    // Adjust the table panel width.
    tablePanel.style.width = ratio + '%';
    // Adjust the gantt panel width.
    ganttPanel.style.left = ratio + '%';
}
// default view ratio
spliterTo(60);

const observer = new MutationObserver(function (mutations) {
    mutations.forEach(function (mutation) {
        if (mutation.attributeName === "value") {
            console.log("mnmn");
        }
    });
});

const dates = document.querySelector('.datepicker');
observer.observe(dates, {
    attributes: true
});