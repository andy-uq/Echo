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
//# sourceMappingURL=Vector.js.map
