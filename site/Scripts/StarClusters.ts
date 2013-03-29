/// <reference path="knockout.d.ts" />
/// <reference path="knockout-postbox.d.ts" />

declare var starClusters: StarCluster[];

class Vector {
    constructor(public x : number, public y : number){
    }

    toString() : string {
        return "(" + this.x + ", " + this.y + ")";
    }
}

interface IStarCluster {
    id: string;
    name: string;
    localCoordinates: Vector;
}

class StarCluster implements IStarCluster {
    id: string;
    name: string;
    localCoordinates: Vector;

    constructor(values: IStarCluster) {
        if (values == null)
            return;

        this.id = values.id;
        this.name = values.name;
        this.localCoordinates = values.localCoordinates;
    }

    size() : string {
        return "N/A";
    }
}

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
    id : string;
    name : KnockoutObservableString;
    localCoordinates : KnockoutObservableAny;

    constructor(starCluster: StarCluster) {
        this.id = starCluster.id;
        this.name = ko.observable(starCluster.name);
        this.localCoordinates = ko.observable(new VectorViewModel(starCluster.localCoordinates));
    }
    
    save(): StarCluster {
        return new StarCluster({
            id: this.id,
            name: this.name(),
            localCoordinates: this.localCoordinates().save()
        });
    }

    update() {
        var starCluster = this.save();
        ko.postbox.publish("starcluster.update", starCluster);
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
    newStarCluster: KnockoutObservableAny;
    editStarCluster: KnockoutObservableAny;

    constructor(starClusters : StarCluster[]) {
        var self = this;

        this.data = ko.observableArray();
        for (var i in starClusters)
            this.data.push(new StarCluster(starClusters[i]))
        
        this.newStarCluster = ko.observable(new StarClusterViewModel(new StarCluster(null)));
        this.editStarCluster = ko.observable();
        this.remove = x => {
            var currentlyEditing = self.editStarCluster();
            if (currentlyEditing && currentlyEditing.id == x.id) {
                self.resetEdit();
            }

            self.data.remove(x);
        }
        this.edit = x => {
            self.newStarCluster(null);
            self.editStarCluster(new StarClusterViewModel(x));
        }

        ko.postbox.subscribe("starcluster.new", x => { self.new(x) });
        ko.postbox.subscribe("starcluster.update", x => { self.update(x) });
    }

    remove(starCluster: StarCluster) { }
    edit(starCluster: StarCluster) { }

    new (starCluster: StarCluster) {
        starCluster.id = "starClusters/2";
        this.data.push(starCluster);
    }

    find(id: string): StarCluster {
        for (var i = 0; i < this.data().length; i++) {
            var value: StarCluster = this.data()[i];
            if (value.id == id)
                return value;
        }

        return null;
    }
         
    update(starCluster: StarCluster) {
        var oldItem = this.find(starCluster.id);
        this.data.replace(oldItem, starCluster);

        this.resetEdit();
    }

    resetEdit() {
        this.editStarCluster(null);
        this.newStarCluster(new StarClusterViewModel(new StarCluster(null)));
    }
}

declare var $;

// Activates knockout.js
$(function () { ko.applyBindings(new StarClusterIndexViewModel(starClusters)) });
