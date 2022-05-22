#ifdef GL_ES
    precision mediump float;
#endif

varying vec4 v_color;
varying vec2 v_texCoords;
varying vec2 v_normTexCoords;

uniform sampler2D u_texture;
uniform float t;

#define PI 3.1415926538
#define strand_interval 3.0

// https://thebookofshaders.com/10/
float random (vec2 st) {
    return fract(sin(dot(st.xy, vec2(12.9898,78.233)))*43758.5453123);
}

float norm_flip(float x, float range) {
    return clamp(1.0 - (x / max(range, 0.01)),0.0,1.0);
}

float delta_angle(float a, float b) {
    float diff = abs(a-b);
    return min(diff, 2.0*PI - diff);
}

float strand_center(float b, float t) {
    float i = floor(t / strand_interval);
    return 2.0*PI*(b + 0.1*random(vec2(b, i)));
}

float strand_t(float t) {
    float i = floor(t / strand_interval);
    return (t - i*strand_interval)/strand_interval;
}

float strand(float b, float t, float theta, float r, float w, float range) {
    w*=2.0;
    float intensity = sin(PI*strand_t(t));
    float angle = mod(t + strand_center(b, t), 2.0*PI);
    float delta_ang = delta_angle(theta, angle);
    float ang_w = max(w*intensity, 0.01);
    return (1.0 - (delta_ang / ang_w))*intensity*norm_flip(r,range)*(delta_ang < ang_w ? 1.0 : 0.0);
}

float ang_mod(float theta) {
    return mod(theta, 2.0*PI);
}

void main() {
    vec2 rel = v_normTexCoords - vec2(0.5);
    rel.x *= 1.15;
    float range = 0.45 - 0.04*(sin(t*4.0)+1.0);
    float theta = atan(rel.y,rel.x) + PI;
    float r = length(rel);
    float r_norm = norm_flip(r, range);

    // long strands
    float long_strand_range = 0.5;
    float long_strand_w = 0.2;
    float strand_acc = 0.0;
    int long_strand_cnt = 4;
    for(int i = 0; i < 4; ++i) {
        strand_acc += 0.8*strand(float(i) / float(long_strand_cnt), t + float(i)*(strand_interval / float(long_strand_cnt)), theta, r, long_strand_w, long_strand_range);
    }
    
    // short strands
    float short_strand_range = 0.35;
    float short_strand_w = 0.5;
    int short_strand_cnt = 4;
    for(int i = 0; i < 4; ++i) {
        strand_acc += strand(float(i) / float(short_strand_cnt), t + float(i)*(strand_interval / float(short_strand_cnt)), ang_mod(theta + PI*0.5), r, short_strand_w, short_strand_range);
    }

    float a = clamp(r_norm + 2.0*strand_acc, 0.0, 1.0);
    gl_FragColor = v_color*vec4(1.0,1.0,1.0,a);

    // debug
    // gl_FragColor = vec4(vec3(strand_acc), 1.0);
}