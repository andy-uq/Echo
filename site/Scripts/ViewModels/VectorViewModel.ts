/// <reference path="../typedef/knockout.d.ts" />
/// <reference path="../typedef/knockout-postbox.d.ts" />

/// <reference path="../Types/Vector.ts" />

class VectorViewModel {
    x: KnockoutObservableNumber;
    y: KnockoutObservableNumber;

    constructor(public vector: Vector) {
        var self = this;

        self.x = ko.observable(0);
        self.y = ko.observable(0);
    }

    reset() {
        this.x(0);
        this.y(0);
    }

    save(): Vector {
        return new Vector(this.x(), this.y());
    }

    toString(): string {
        return this.save().toString();
    }
}