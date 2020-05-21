
var preRowsNo = 0;
google.charts.load('current', {'packages':['gantt']});
//google.charts.setOnLoadCallback(drawChart);

class Dependency {
    constructor(id, dep, lag) {
        this.id = id;
        this.Dependency = dep;
        this.lag = lag;
    }
}

class Task {
    constructor(id, name, start, end, duration) {
        this.id = id;
        this.name = name;
        //this.progress = 0;
        //this.status = "STATUS_UNDEFINED";
        //this.depends = "";

        this.start = start.getTime();
        this.end = end.getTime();
        this.duration = duration;

        //permissions
        // by default all true, but must be inherited from parent
        //this.canWrite = true;
        //this.canAdd = true;
        //this.canDelete = true;
        //this.canAddIssue = true;

        //this.rowElement; //row editor html element
        //this.ganttElement; //gantt html element
        //this.master;


        //this.dependencies = [];
        //this.assignees = [];
    }
}

var dep = new Dependency("Dependency0", 2, 2);

//document.onload = table(edit);
function table(dataArray,noData) {
    if (noData) {
        if (!dataArray.length > 0) {
            createRowTask(10);
        } else {
            createRowTaskWithData(dataArray);
            createRowTask(1);
        }
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
    if (h_date.getDay() == 5 || h_date.getDay() == 6) {
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

pickedCells = [];
/* #region  previous function */


// the body
function createRowTask(num) {
    num = num + preRowsNo;
    for (let n = preRowsNo; n < num; n++) {
        var datapt1 = `<td><label>${n + 1}</label></td>
        <td><input type="text" id="taskName${n}" style="background-color: transparent; border: none;" class="Name test no-outline"></td>`;

        var td1 = $('<td></td>');
        var datepicker1 = $(`<input type="text"  id="startDate${n}" class="datepicker test no-outline" readonly>`);
        datepicker1.datepicker({
            disabled: true,
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy',
            firstDay: 0,   //---Set The First Day in the Week ( Saturday)
            beforeShowDay: function(dt){
                return [dt.getDay() == 5 || dt.getDay() == 6 ? false : true];
            }, //----   (Disable The Week Ends in Datepicker  )
            minDate: 0, //-----Disable the previous Days 
            onSelect: function () {
                var startDate = $(`#startDate${n}`).datepicker('getDate');
                var endDate = $(`#endDate${n}`).datepicker('getDate');
                var Duration = $(`#Duration${n}`)
                if (startDate && endDate) {
                    Duration.val(computeDuration(startDate, endDate));
                    // var selectedId = tasks.findIndex(x => x.id === `${n}`);
                    // console.log(selectedId);
                }
            }
        });
        td1.append(datepicker1);

        var td2 = $('<td></td>');
        var datepicker2 = $(`<input type="text"  id="endDate${n}" class="datepicker test no-outline" readonly>`);
        datepicker2.datepicker({
            disabled: true,
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy',
            firstDay: 0,   //---Set The First Day in the Week ( Saturday)
            beforeShowDay: function(dt){
                return [dt.getDay() == 5 || dt.getDay() == 6 ? false : true];
            }, //----   (Disable The Week Ends in Datepicker  )
            minDate: 0, //-----Disable the previous Days 
            onSelect: function () {
                var startDate = $(`#startDate${n}`).datepicker('getDate');
                var endDate = $(`#endDate${n}`).datepicker('getDate');
                var Duration = $(`#Duration${n}`)
                if (startDate && endDate) {
                    Duration.val(computeDuration(startDate, endDate));
                    // var selectedId = tasks.findIndex(x => x.id === `${n}`);
                    // console.log(selectedId);
                }
            }
        });
        td2.append(datepicker2);

        var datapt2 = `<td><input type="text" id="Duration${n}" class="durations test no-outline" readonly></td>
        <td><input type="text" id ="Dependency${n}" onchange="dependency(this.id,this.value)"
            class="test no-outline" onkeyup="wirteDependency(this)" readonly>
        </td>
        <td><input type="text" class="test no-outline" readonly></td>
        <td><input type="text" class="test no-outline" readonly></td>`;

        var tr = $(`<tr></tr>`)
        tr.attr("id", `tempT-${n}`);
        tr.append(datapt1);
        tr.append(td1);
        tr.append(td2);
        tr.append(datapt2);
        $('#tbody').append(tr);
    }
    preRowsNo = num;
}

var tasks = [];
function createTaskObj(n) {
    var taskId = n;
    var taskName = $(`#taskName${n}`)[0].value;
    var Sdate = $(`#startDate${n}`)[0].value;
    Sdate = parseDate(Sdate);
    var Edate = $(`#endDate${n}`)[0].value;
    Edate = parseDate(Edate);
    var Duration = $(`#Duration${n}`)[0].value;

    var task = new Task(taskId, taskName, Sdate, Edate, Duration);
    //console.log(task);
    return task;
}

function replaceTask(n) {
    var newTask = createTaskObj(n);
    var index = tasks.findIndex(t => t.id == n);
    tasks.splice(index, 1, newTask);
    //console.log(tasks);
}

var tasksId = [];
function assignTasks() {
    $('.Name').each(function () {
        if (this.value) {
            var taskId = this.id.toString().split('taskName')[1];
            if (!tasksId.includes(taskId)) {
                tasksId.push(taskId);
                var task = createTaskObj(taskId);
                tasks.push(task);
                //console.log(tasks);
            } else {
                replaceTask(taskId);
            }
        }
    });
    //console.log(tasksId);
    var finalTasks = tasks;
    return finalTasks;
}


var preTaskName = "";
filledRows = [];
$('tbody').on('change', '.Name', function (e) {
    e.preventDefault();
    if (!preTaskName) {
        var id = e.target.id;
        var n = id.toString().split("taskName")[1];
        n = parseInt(n);
        openRow(n);
        fillRowTask(e);
        if (!filledRows.includes(n)) { createRowTask(1); }
        filledRows.push(n);
    } else {
        var id = e.target.id;
        var n = id.toString().split("taskName")[1];
        var name = $(`#taskName${n}`)[0].value;
        var selectedId = tasks.findIndex(x => x.id === `${n}`);
        console.log(selectedId);
    }
});

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
    //assignTask(n);
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
        });
    });
    $(`#startDate${id}`).datepicker("option", "disabled", false);
    $(`#endDate${id}`).datepicker("option", "disabled", false);
}

//complete one senario
function dependency(id, dep) {
    if (!dep) { return }
    var n = id.toString().split("Dependency")[1];
    var deps = dep.toString().split(':');
    var targetLag = parseInt(deps[1]);
    console.log(deps);
    var targetEndDate = parseDate($(`#endDate${deps[0] - 1}`)[0].value);
    var startDate = parseDate($(`#startDate${n}`)[0].value);
    //var endDate = parseDate($(`#endDate${n}`)[0].value);
    var preDurat = $(`#Duration${n}`)[0];
    console.log(preDurat);
    var lag = Math.ceil((startDate - targetEndDate) / 86400000);
    console.log("lag = " + lag);
    if (lag < 0) {
        if (targetLag) { lag = lag - targetLag - 1; }
        console.log("lag += " + lag)
        var d1 = new Date(startDate);
        //var d2 = new Date(endDate);
        while (lag < 0) {
            if (!isHoliday(d1)) {
                lag++;
            }
            d1.setDate(d1.getDate() + 1);
            //d2.setDate(d2.getDate() + 1);
        }
        while (isHoliday(d1)) { d1.setDate(d1.getDate() + 1); }
        var SDate = document.getElementById(`startDate${n}`);
        SDate.value = formatDate(d1);
        var EDate = document.getElementById(`endDate${n}`);
        var re = computeEndFromDuration(d1, preDurat);
        EDate.value = formatDate(re);
    }

    if (lag > 0) {
        if(targetLag){lag = lag + targetLag;}
    }
}

function wirteDependency(input){
    var regex = /[^0-9:]/gi;
    input.value = input.value.replace(regex, "");
}

function daysToMilliseconds(days) {
    return days * 24 * 60 * 60 * 1000;
}

function drawChart() {

    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Task ID');
    data.addColumn('string', 'Task Name');
    data.addColumn('date', 'Start Date');
    data.addColumn('date', 'End Date');
    data.addColumn('number', 'Duration');
    data.addColumn('number', 'Percent Complete');
    data.addColumn('string', 'Dependencies');

    for (let i = 0; i < tasks.length; i++) {
        var dep = `${tasks[i].depends}`;
        if(dep<0){dep = null}
        data.addRows([
            [`${tasks[i].id}`, `${tasks[i].name}`, tasks[i].startDate, tasks[i].endDate,
            daysToMilliseconds(tasks[i].duration), 20, dep]
        ]);

    }
 

    var options = {
        height: 275,
        gantt: {
            criticalPathEnabled: false,
            criticalPathStyle: {
                stroke: '#e64a19',
                strokeWidth: 5
            }
        }
    };

    var chart = new google.visualization.Gantt(document.getElementById('chart_div'));

    chart.draw(data, options);
}

// send the array of tasks to the backend
function submit() {
    var proId = $('#proId').val();
    var Acts = assignTasks();
    
    $.ajax({
        type: "POST",
        url: "/Activity/AddActivities",
        data: { id: proId, Acts: Acts },
        success: function (response) {
            console.log("fol");
        },
        error: function (x, y, err) {
            console.log(arguments);
        }
    });
}
///////////////////////////////////

/**/
