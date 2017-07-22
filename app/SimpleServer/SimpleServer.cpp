#include <iostream>
#include <sys/socket.h>
#include <cassert>
#include <string.h>

using namespace std;

#define LOG(m) { cout<< m << endl; }
#define CHK_ERR(x) { \
 if (!(x)) { \
    LOG("err: "<< errno << " " << strerror(errno) << " " << __LINE__); \
    exit(1);  \
   } \
}

void start_server(int port) {
    LOG("creating socket on port " << port);
    int fd = ::socket(AF_INET,SOCK_STREAM,0);
    CHK_ERR(fd > 0);
    while(true) {
      sockaddr addr;
      socklen_t addr_len=sizeof(addr);
      auto conn = ::accept(fd, &addr, &addr_len);
      LOG("client connected at port " << port); 
      CHK_ERR(conn > 0);
   }

}


int main() {
  start_server(31017);  

}
