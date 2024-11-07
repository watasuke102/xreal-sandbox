LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)

SRC_DIR          := ../../src
LOCAL_MODULE     := glesdraw_android
LOCAL_SRC_FILES  := $(SRC_DIR)/lib.cpp $(SRC_DIR)/shader.cpp
LOCAL_LDLIBS     += -lGLESv3 -rdynamic -fno-exceptions -fno-rtti
LOCAL_CPPFLAGS   += -g -I../../inc -std=c++17 -fdiagnostics-color=always -Wall -Winvalid-pch -Wextra -Wpedantic -lGLESv3

include $(BUILD_SHARED_LIBRARY)
