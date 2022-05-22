#ifdef GL_ES
    precision mediump float;
#endif

varying vec2 v_texCoords;

uniform sampler2D u_texture;
uniform sampler2D mask;
uniform float t;

void main() {
    // ACTUAL INPUTS (should be uniforms probably)
    float spinniness = 20.0;
    float fade_range = 0.01;
    float mask_influence = 0.1;
    float streak_influence = 0.5;

    float t_used = smoothstep(0.0,1.0,t);

    vec2 rel = v_texCoords - vec2(0.5,0.5);
    float r = length(rel);
    float mask_sample = texture2D(mask, v_texCoords).x;
    float theta = atan(rel.y, rel.x);
    float r_fac = (sqrt(0.5) - r)/sqrt(0.5);
    float streak = sin(1.0*theta + spinniness*(r + mask_influence*mask_sample));
    float streak01 = (streak+1.0)*0.5;
    float r_fac_with_streak = r_fac*r_fac*(streak01*streak_influence + 1.0 - streak_influence);
    float m = clamp(r_fac_with_streak,0.0,1.0);
    
    float top = (1.0+fade_range)*t_used;
    float bot = top - fade_range;
    float a = clamp((m - bot) / fade_range,0.0,1.0);
    gl_FragColor = a*texture2D(u_texture, v_texCoords);
    // gl_FragColor = vec4(m);
}