
all: libs/arm64-v8a/libcppadd-aarch64.so libs/libcppadd-x86_64.so

libs/arm64-v8a/libcppadd-aarch64.so: main.cpp
	cd jni && $(NDK)/ndk-build

libs/libcppadd-x86_64.so: main.cpp
	clang++ -shared -fno-exceptions -fno-rtti -undefined dynamic_lookup -o $@ $<
