/// <reference path="knockout.d.ts" />
/// <reference path="knockout-postbox.d.ts" />

class Vector {
    constructor(public x : number, public y : number){
    }

    toString() : string {
        return "(" + this.x + ", " + this.y + ")";
    }
}

class StarCluster {
    constructor(public name : string, public localCoordinates : Vector) {}

    size() : string {
        return "N/A";
    }
}

var starClusters : StarCluster[] = [];

class VectorViewModel {
    x : KnockoutObservableNumber;
    y : KnockoutObservableNumber;

    constructor(public vector : Vector)
    {
        var self = this;

        self.x = ko.observable(0);
        self.y = ko.observable(0);
    }   

    reset(){
        this.x(0);
        this.y(0);
    }

    save() : Vector {
        return new Vector(this.x(), this.y());
    }
}

class StarClusterViewModel {
    name : KnockoutObservableString;
    localCoordinates : KnockoutObservableAny;

    constructor(starCluster : StarCluster){
        this.name = ko.observable(starCluster.name);
        this.localCoordinates = ko.observable(new VectorViewModel(starCluster.localCoordinates));
    }
    
    save() : StarCluster {
        return new StarCluster(this.name(), this.localCoordinates().save());
    }

    add() {
        var starCluster = this.save();
        ko.postbox.publish("starcluster.new", starCluster);
        this.reset();
    }

    reset() {
        this.name("");
        this.localCoordinates().reset()
    }
}

class StarClusterIndexViewModel
{
    data : KnockoutObservableArray;
    newStarCluster: StarClusterViewModel;

    constructor(starClusters : StarCluster[]) {
        var self = this;

        this.data = ko.observableArray(starClusters);
        this.newStarCluster = new StarClusterViewModel(new StarCluster("", new Vector(0, 0)));

        ko.postbox.subscribe("starcluster.new", x => { self.new(x) });
    }

    new(starCluster:StarCluster) {
        this.data.push(starCluster);
    }
}


// Activates knockout.js
ko.applyBindings(new StarClusterIndexViewModel([]));