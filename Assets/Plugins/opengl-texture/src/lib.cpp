#include "shader.hpp"
#include <GLES3/gl3.h>
#include <cstddef>
#include <cstdint>

namespace {
#define GLSL(s) "#version 310 es\n" #s

constexpr char vertex_shader[] = GLSL(

    layout(location = 0) in vec3 position;

    uniform mat4 mvp; out mediump vec3 pos;

    void main() {
      gl_Position = mvp * vec4(position, 1.0);
      pos         = gl_Position.xyz;
    }

);
constexpr char flagment_shader[] = GLSL(

    in mediump vec3 pos; out mediump vec4 color;

    void main() {
      mediump vec3 threshold = vec3(0);
      mediump vec3 filtered  = step(threshold, pos);
      color                  = vec4(filtered, 1.0);
    }

);

GLuint program_id;
GLuint vert_buffer, elem_buffer;

constexpr size_t vert_len = 4;
constexpr size_t elem_len = 3;
} // namespace

struct Pos {
  GLfloat x, y, z;
};

struct Edge {
  // contain index
  GLuint begin, end;
};

extern "C" {
void init() {
  program_id = compile_shader(vertex_shader, flagment_shader);

  glGenBuffers(1, &vert_buffer);
  glGenBuffers(1, &elem_buffer);

  {
    GLsizeiptr size = sizeof(Pos) * vert_len;
    glBindBuffer(GL_ARRAY_BUFFER, vert_buffer);
    glBufferData(GL_ARRAY_BUFFER, size, nullptr, GL_STATIC_DRAW);
    Pos* pos =
        (Pos*)glMapBufferRange(GL_ARRAY_BUFFER, 0, size, GL_MAP_WRITE_BIT);
    float z  = 0.f;
    pos[0].x = 0.5f;
    pos[0].y = 0.5f;
    pos[0].z = z;
    pos[1].x = 0.5f;
    pos[1].y = -0.5f;
    pos[1].z = z;
    pos[2].x = -0.5f;
    pos[2].y = -0.5f;
    pos[2].z = z;
    pos[3].x = -0.5f;
    pos[3].y = 0.5f;
    pos[3].z = z;
    // needless?
    glUnmapBuffer(GL_ARRAY_BUFFER);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
  }

  {
    GLsizeiptr size = sizeof(Edge) * elem_len;
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, elem_buffer);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, size, nullptr, GL_STATIC_DRAW);
    Edge* edge = (Edge*)glMapBufferRange(
        GL_ELEMENT_ARRAY_BUFFER, 0, size, GL_MAP_WRITE_BIT);
    edge[0].begin = 1;
    edge[0].end   = 2;
    edge[1].begin = 2;
    edge[1].end   = 3;
    edge[2].begin = 3;
    edge[2].end   = 0;
    glUnmapBuffer(GL_ELEMENT_ARRAY_BUFFER);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
  }
}

int draw(float x0, float y0, float z0, float w0, //
         float x1, float y1, float z1, float w1, //
         float x2, float y2, float z2, float w2, //
         float x3, float y3, float z3, float w3  //
) {
  static uint64_t i;
  float           mvp_mat[] = {
      x0, y0, z0, w0, x1, y1, z1, w1, x2, y2, z2, w2, x3, y3, z3, w3};

  glUseProgram(program_id);

  GLuint matrix_id = glGetUniformLocation(program_id, "mvp");
  glUniformMatrix4fv(matrix_id, 1, GL_FALSE, mvp_mat);

  glEnableVertexAttribArray(0);
  glBindBuffer(GL_ARRAY_BUFFER, vert_buffer);
  glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 0, 0);

  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, elem_buffer);
  glDrawElements(GL_TRIANGLE_FAN, elem_len * 2, GL_UNSIGNED_INT, 0);

  glBindBuffer(GL_ARRAY_BUFFER, 0);
  glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
  glDisableVertexAttribArray(0);
  glFlush();
  return i++;
}
}
