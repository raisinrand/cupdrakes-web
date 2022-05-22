#ifdef GL_ES
    precision mediump float;
#endif

varying vec2 v_texCoords;

uniform sampler2D u_texture;

void main() {
    vec2 texCoord = mod(v_texCoords, 1.0);
    gl_FragColor = texture2D(u_texture, texCoord);
}