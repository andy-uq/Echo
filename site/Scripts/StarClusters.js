var Vector = (function () {
    function Vector(x, y) {
        this.x = x;
        this.y = y;
    }
    Vector.prototype.toString = function () {
        return "(" + this.x + ", " + this.y + ")";
    };
    return Vector;
})();
var StarCluster = (function () {
    function StarCluster(name, localCoordinates) {
        this.name = name;
        this.localCoordinates = localCoordinates;
    }
    StarCluster.prototype.size = function () {
        return "N/A";
    };
    return StarCluster;
})();
var starClusters = [];
var VectorViewModel = (function () {
    function VectorViewModel(vector) {
        this.vector = vector;
        var self = this;
        self.x = ko.observable(0);
        self.y = ko.observable(0);
    }
    VectorViewModel.prototype.reset = function () {
        this.x(0);
        this.y(0);
    };
    VectorViewModel.prototype.save = function () {
        return new Vector(this.x(), this.y());
    };
    return VectorViewModel;
})();
var StarClusterViewModel = (function () {
    function StarClusterViewModel(starCluster) {
        this.name = ko.observable(starCluster.name);
        this.localCoordinates = ko.observable(new VectorViewModel(starCluster.localCoordinates));
    }
    StarClusterViewModel.prototype.save = function () {
        return new StarCluster(this.name(), this.localCoordinates().save());
    };
    StarClusterViewModel.prototype.add = function () {
        var starCluster = this.save();
        ko.postbox.publish("starcluster.new", starCluster);
        this.reset();
    };
    StarClusterViewModel.prototype.reset = function () {
        this.name("");
        this.localCoordinates().reset();
    };
    return StarClusterViewModel;
})();
var StarClusterIndexViewModel = (function () {
    function StarClusterIndexViewModel(starClusters) {
        var self = this;
        this.data = ko.observableArray(starClusters);
        this.newStarCluster = new StarClusterViewModel(new StarCluster("", new Vector(0, 0)));
        ko.postbox.subscribe("starcluster.new", function (x) {
            self.new(x);
        });
    }
    StarClusterIndexViewModel.prototype.new = function (starCluster) {
        this.data.push(starCluster);
    };
    return StarClusterIndexViewModel;
})();
ko.applyBindings(new StarClusterIndexViewModel([]));
