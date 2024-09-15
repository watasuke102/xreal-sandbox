LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)

LOCAL_MODULE     := cppadd-aarch64
LOCAL_SRC_FILES  := ../main.cpp
LOCAL_CPPFLAGS   += -std=c++17 -fno-exceptions -fno-rtti

include $(BUILD_SHARED_LIBRARY)
