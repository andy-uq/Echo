class Vector {
    constructor(public x: number, public y: number) {
    }

    toString(): string {
        return "(" + this.x + ", " + this.y + ")";
    }
}