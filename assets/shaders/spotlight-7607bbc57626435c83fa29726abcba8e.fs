#ifdef GL_ES
    precision mediump float;
#endif

varying vec4 v_color;
varying vec2 v_texCoords;
varying vec2 v_normTexCoords;

uniform sampler2D u_texture;
uniform vec2 screenSize;
uniform vec2 center;
uniform float radius;
uniform float fade_range;


float invlerp(float a, float b, float x) {
    return clamp((x - a) / (b - a),0.0,1.0);
}

void main() {
    vec2 rel = v_normTexCoords*screenSize - center;
    float r = length(rel);

    float top = radius + fade_range;
    float bot = radius;
    float a = invlerp(bot, top, r);

    vec4 res = v_color;
    res.a *= a;
    gl_FragColor = res;
}