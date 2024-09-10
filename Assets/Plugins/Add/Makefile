CXX := $(NDK)/toolchains/llvm/prebuilt/linux-x86_64/bin/aarch64-linux-android26-clang++
CXXFLAGS := -shared -fno-exceptions -fno-rtti -undefined dynamic_lookup

libcppadd.so: main.cpp
	$(CXX) $(CXXFLAGS) -o $@ $<
