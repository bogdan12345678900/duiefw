(function ($) {
    const v = $.validator, a = v.unobtrusive.adapters;

    v.addMethod("releaseyear", function (value, el, p) {
        if (!value) return true;
        const y = parseInt(value, 10);
        return !isNaN(y) && y >= p.min && y <= p.max;
    });
    a.add("releaseyear", ["min", "max"], function (o) {
        o.rules["releaseyear"] = { min: parseInt(o.params.min, 10), max: parseInt(o.params.max, 10) };
        o.messages["releaseyear"] = o.message;
    });

    v.addMethod("noforbiddenwords", function (value, el, p) {
        if (!value) return true;
        const t = value.toLowerCase();
        return !p.words.split(",").some(w => w.trim() && t.includes(w.trim().toLowerCase()));
    });
    a.add("noforbiddenwords", ["words"], function (o) {
        o.rules["noforbiddenwords"] = { words: o.params.words };
        o.messages["noforbiddenwords"] = o.message;
    });

    // Десятичная запятая в поле «Оценка»
    v.methods.number = function (value, el) {
        return this.optional(el) || /^-?\d+([.,]\d+)?$/.test(value);
    };
    v.methods.range = function (value, el, p) {
        return this.optional(el) || (parseFloat(value.replace(",", ".")) >= p[0] && parseFloat(value.replace(",", ".")) <= p[1]);
    };
})(jQuery);
