//google.charts.load('current', { 'packages': ['corechart'] });
//google.charts.setOnLoadCallback(drawChart);


//function drawChart() {
//    var data = google.visualization.arrayToDataTable([
//        ["Element", "Density", { role: "style" }],
//        ["Copper", 8.94, "#b87333"],
//        ["Silver", 10.49, "silver"],
//        ["Gold", 19.30, "gold"],
//        ["Platinum", 21.45, "color: #e5e4e2"]
//    ]);

//    var view = new google.visualization.DataView(data);
//    view.setColumns([0, 1,
//        {
//            calc: "stringify",
//            sourceColumn: 1,
//            type: "string",
//            role: "annotation"
//        },
//        2]);

//    var options = {
//        title: "Tasks in progress",
//        width: 200,
//        height: 150,
//        bar: { groupWidth: "70%", bars: 'vertical' },
//        legend: { position: "none" },
//        // bars: 'vertical'
//    };
//    var chart = new google.visualization.BarChart(document.getElementById("barchart_values"));
//    chart.draw(view, options);
//}

//drawChart();