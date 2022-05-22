#ifdef GL_ES
    precision mediump float;
#endif

#define PI 3.14159

varying vec4 v_color;
varying vec2 v_texCoords;
varying vec2 v_normTexCoords;

uniform sampler2D u_texture;

uniform float time;
uniform float open;
uniform float wiggle;

float quart(float x) {
    return x*x*x*x;
}

// https://easings.net/#easeOutElastic
float easeOutElastic(float t) {
    t = clamp(t, 0.0,1.0);
    float c4 = (2.0 * PI) / 3.0;
    return t == 0.0
        ? 0.0
        : t == 1.0
        ? 1.0
        : pow(2.0, -10.0 * t) * sin((t * 10.0 - 0.75) * c4) + 1.0;
}

float easeOutBack(float x) {
    float c1 = 1.70158;
    float c3 = c1 + 1.0;
    return 1.0 + c3 * pow(x - 1.0, 3.0) + c1 * pow(x - 1.0, 2.0);
}

float ease(float x) {
    float omx = (1.0-x);
    float a = 1.0 - omx*omx*omx*omx*omx;
    float s = -0.2*x*(0.1*(cos(2.0*PI*x) + x*omx));
    return (a + s)/0.98;
}

void main() {
    float open_t = ease(open);
    float w = open_t;
    float norm_x = clamp(v_normTexCoords.x / w, 0.0, 1.0);
    // float t_for_h = smoothstep(0.0,1.0,open_t);
    float t_for_h = open_t;
    float ht = 1.0-v_normTexCoords.x;
    float hh = 1.0 * mix(1.0,t_for_h,quart(ht)) * mix(1.0 - quart(norm_x), 1.0, t_for_h);

    float theta = v_normTexCoords.x*8.0 + time;
    vec2 texCoord = v_texCoords;
    texCoord.x /= w;
    texCoord.y = (texCoord.y - 0.5)/hh + 0.5;

    // wiggle
    texCoord += wiggle*vec2(cos(theta),sin(theta));
    
    vec4 col = v_color*texture2D(u_texture, texCoord);
    gl_FragColor = col;
}