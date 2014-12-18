
function reset(){
  var options = {};
  options.width = 1800;
  options.height = 1800;
  options.charge = -5000;
  options.linkDistance = 300;
  options.gravity = 5;
  options.Field = "Followers";
  return options;

}

function clearGraph(){
  $("#svgCanvas").remove();
}

function getInputValue(id, fallback)
{
    var s = $('#'+id);
    return s.val() || fallback;
}

function drawGraph() {

  clearGraph();

  var options = reset();
  options.fieldToHighlight = getInputValue("txtUsername", "");
  options.width = options.height = getInputValue("rgWidth", options.width);
  options.charge = getInputValue("rgCharge", options.charge);
  options.linkDistance = getInputValue("rgLinkDistance", options.linkDistance);
  options.gravity = getInputValue("rgGravity", options.gravity);
  options.field = getInputValue("drpField", options.field);
  options.domainRange=[0,1100];

  options.scaledcolor = d3.scale.category20();
  options.scale = d3.scale.linear()
    .domain(options.domainRange)
    .range([0,20]);


  options.color = function(v) {    
    return options.scaledcolor(Math.floor(options.scale(v)));
  };
  

  $.getJSON("./hubski2.json", function(data) {
    renderGraph(data,options);
  });

  function renderGraph(data, options){
    var width = options.width,
      height = options.height;
    
    var force = d3.layout.force()
      .charge(options.charge)
      .linkDistance(options.linkDistance)
      .gravity(options.gravity)
      .size([width, height]);
    
    var svg = d3.select("body").append("svg")
      .attr("width", width)
      .attr("height", height)
      .attr("id", "svgCanvas");
    
    var graph = data;
    var nodeMap = {};
    graph.nodes.forEach(function(d) { 
      nodeMap[d.Name] = d; 
    });

    
    var links = [];
    graph.links.forEach(function(l) {
      l.source = nodeMap[l.source];
      l.target = nodeMap[l.target];
      if (l.source && l.target) {
        links.push(l);
      }
    });
    graph.links = links;
    
    force.nodes(graph.nodes)
      .links(graph.links)
      .start();
    
    var link = svg.selectAll(".link")
      .data(graph.links)
      .enter().append("line")
      .attr("class", "link")
      .style("stroke-width", function(d) {
        return d.Value / 10;
      });
    
    var node = svg.selectAll(".node")
      .data(graph.nodes)
      .enter().append("circle")
      .attr("class", "node")
      .attr("r", function(d) {
        return Math.sqrt(d[options.field]);
      })
      .style("fill", function(d) {
        if(options.fieldToHighlight.toLowerCase() === d.Name.toLowerCase()){
          return "#CC0066";
        }
        else{
          return options.color(d[options.field]);
        }
      })
      .style("stroke", function(d) {
        if(options.fieldToHighlight.toLowerCase() === d.Name.toLowerCase()){
          return "#99FF33";
        }
        else{
          return "#000";
        }
      })
      .style("stroke-width", function(d) {
        if(options.fieldToHighlight.toLowerCase() === d.Name.toLowerCase()){
          return 3;
        }
        else{
          return 1;
        }
      })
      .attr("id", function(d) {
        return "node_" + d.Name.toLowerCase();
      })
      .call(force.drag); //
    

    node.append("title")
      .text(function(d) {
        return d.Name + " : " + d[options.field];
      });
    

    force.on("tick", function() {
      link.attr("x1", function(d) {
          return d.source.x;
        })
        .attr("y1", function(d) {
          return d.source.y;
        })
        .attr("x2", function(d) {
          return d.target.x;
        })
        .attr("y2", function(d) {
          return d.target.y;
        });
      node.attr("cx", function(d) {
          return d.x;
        })
        .attr("cy", function(d) {
          return d.y;
        });
    });
  }
}