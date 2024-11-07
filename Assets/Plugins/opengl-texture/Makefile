.PHONY: all clean
OBJ_DIR  := build/obj
SOURCES  := $(wildcard src/*.cpp)
OBJS     := $(addprefix $(OBJ_DIR)/, $(patsubst src/%.cpp, %.o, $(SOURCES)))
CXXFLAGS := -g -Iinc -fdiagnostics-color=always -Wall -Winvalid-pch -Wextra -Wpedantic
LDFLAGS  := -shared -rdynamic -fno-exceptions -fno-rtti

# For Unity Editor
X86_64_LIB  = build/libglesdraw.so
# For Android
AARCH64_LIB = build/libs/arm64-v8a/libglesdraw_android.so

TARGET_LIBS = $(X86_64_LIB) $(AARCH64_LIB)
all: $(TARGET_LIBS)

$(OBJ_DIR):
	mkdir -p $@
$(X86_64_LIB): $(OBJS)
	clang++ $(CXXFLAGS) $(LDFLAGS) -lGL -o $@ $^
$(AARCH64_LIB): src/lib.cpp
	cd build/jni && $(NDK)/ndk-build
	rm build/obj/local/arm64-v8a/libglesdraw_android.so

$(OBJ_DIR)/%.o: src/%.cpp
	clang++ $(CXXFLAGS) -c -o $@ $^

clean:
	rm -vf $(TARGET_LIBS) $(OBJS)
	cd build/jni && $(NDK)/ndk-build clean
