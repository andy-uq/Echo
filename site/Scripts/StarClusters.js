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
    function StarCluster(values) {
        if(values == null) {
            return;
        }
        this.id = values.id;
        this.name = values.name;
        this.localCoordinates = values.localCoordinates;
    }
    StarCluster.prototype.size = function () {
        return "N/A";
    };
    return StarCluster;
})();
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
        this.id = starCluster.id;
        this.name = ko.observable(starCluster.name);
        this.localCoordinates = ko.observable(new VectorViewModel(starCluster.localCoordinates));
    }
    StarClusterViewModel.prototype.save = function () {
        return new StarCluster({
            id: this.id,
            name: this.name(),
            localCoordinates: this.localCoordinates().save()
        });
    };
    StarClusterViewModel.prototype.update = function () {
        var starCluster = this.save();
        ko.postbox.publish("starcluster.update", starCluster);
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
        this.data = ko.observableArray();
        for(var i in starClusters) {
            this.data.push(new StarCluster(starClusters[i]));
        }
        this.newStarCluster = ko.observable(new StarClusterViewModel(new StarCluster(null)));
        this.editStarCluster = ko.observable();
        this.remove = function (x) {
            var currentlyEditing = self.editStarCluster();
            if(currentlyEditing && currentlyEditing.id == x.id) {
                self.resetEdit();
            }
            self.data.remove(x);
        };
        this.edit = function (x) {
            self.newStarCluster(null);
            self.editStarCluster(new StarClusterViewModel(x));
        };
        ko.postbox.subscribe("starcluster.new", function (x) {
            self.new(x);
        });
        ko.postbox.subscribe("starcluster.update", function (x) {
            self.update(x);
        });
    }
    StarClusterIndexViewModel.prototype.remove = function (starCluster) {
    };
    StarClusterIndexViewModel.prototype.edit = function (starCluster) {
    };
    StarClusterIndexViewModel.prototype.new = function (starCluster) {
        starCluster.id = "starClusters/2";
        this.data.push(starCluster);
    };
    StarClusterIndexViewModel.prototype.find = function (id) {
        for(var i = 0; i < this.data().length; i++) {
            var value = this.data()[i];
            if(value.id == id) {
                return value;
            }
        }
        return null;
    };
    StarClusterIndexViewModel.prototype.update = function (starCluster) {
        var oldItem = this.find(starCluster.id);
        this.data.replace(oldItem, starCluster);
        this.resetEdit();
    };
    StarClusterIndexViewModel.prototype.resetEdit = function () {
        this.editStarCluster(null);
        this.newStarCluster(new StarClusterViewModel(new StarCluster(null)));
    };
    return StarClusterIndexViewModel;
})();
$(function () {
    ko.applyBindings(new StarClusterIndexViewModel(starClusters));
});
