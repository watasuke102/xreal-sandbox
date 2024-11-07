#include "shader.hpp"

namespace {
int compile(GLuint program_id, int type, const char* src) {
  GLuint shader_id = glCreateShader(type);

  glShaderSource(shader_id, 1, &src, nullptr);
  glCompileShader(shader_id);

  GLint compiled = GL_FALSE;
  glGetShaderiv(shader_id, GL_COMPILE_STATUS, &compiled);
  if (!compiled) {
    int log_length = 0;
    glGetShaderiv(shader_id, GL_INFO_LOG_LENGTH, &log_length);

    // std::vector<char> log_message(log_length);
    // glGetShaderInfoLog(shader_id, log_length, NULL, &log_message[0]);
    // std::cout << "[Shader error]" << std::endl
    //           << std::string(log_message.begin(), log_message.end())
    //           << std::endl;

    glDeleteShader(shader_id);
    glDeleteProgram(program_id);
    return -1;
  }

  glAttachShader(program_id, shader_id);
  glDeleteShader(shader_id);
  return program_id;
}
} // namespace

GLuint compile_shader(const char* vertex_src, const char* flagment_src) {
  GLuint program_id   = glCreateProgram();
  GLint  link_success = GL_FALSE;
  int    shader_id;

  shader_id = compile(program_id, GL_VERTEX_SHADER, vertex_src);
  if (shader_id == -1) {
    // std::cout << "Failed to compile vertex shader" << std::endl;
    goto err;
  }

  shader_id = compile(program_id, GL_FRAGMENT_SHADER, flagment_src);
  if (shader_id == -1) {
    // std::cout << "Failed to compile flagment shader" << std::endl;
    goto err;
  }

  glLinkProgram(program_id);
  glGetProgramiv(program_id, GL_LINK_STATUS, &link_success);
  if (!link_success) {
    // std::cout << "Failed to link shader" << std::endl;
    goto err;
  }

  return program_id;

err:
  glDeleteProgram(program_id);
  return 0;
}
