#ifdef GL_ES
    precision mediump float;
#endif

varying vec4 v_color;
varying vec2 v_texCoords;
varying vec2 v_normTexCoords;

uniform sampler2D u_texture;

uniform vec2 regionSize;
uniform float deadness;
uniform sampler2D noise;
uniform float squish;
uniform float squishFloor;

void main() {
    vec2 texCoord = v_texCoords;
    texCoord.y -= regionSize.y*clamp(v_normTexCoords.y-squishFloor,0.0,1.0)*squish;
    vec4 col = v_color*texture2D(u_texture, texCoord);

    // deadness
    float deadnoise = 0.25*texture2D(noise, 3.5*v_normTexCoords).x;
    float deadfac = deadness + deadnoise;
    float spread = 0.1;

    float dark = clamp(1.0 + spread- (1.0-v_normTexCoords.y) - (1.0 + spread)*deadfac,0.0,spread)/spread;
    col.rgb *= dark;
    gl_FragColor = col;
}