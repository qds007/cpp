#include <iostream>
#include <sys/socket.h>
#include <cassert>
#include <string.h>
#include <arpa/inet.h>
#include <pthread.h>

using namespace std;

#define LOG(m) { cout<< m << endl; }
#define CHK_ERR(x) { \
 if (!(x)) { \
    LOG("err: "<< errno << " " << strerror(errno) << " " << __LINE__); \
    exit(1);  \
   } \
}
// g++ -o SimpleServer -lpthread  SimpleServer.cpp 

void *start_server(void* arg) {
    int port= *( (int*) arg);
    LOG("creating socket on port " << port);
    int fd = ::socket(AF_INET,SOCK_STREAM,0);
    CHK_ERR(fd > 0);
    int opt_val_ptr;
    CHK_ERR(::setsockopt(fd,SOL_SOCKET,SO_REUSEADDR, &opt_val_ptr,sizeof(int)) ==0 );
    const auto listen_addr=sockaddr_in{ AF_INET, htons(port), in_addr{ INADDR_ANY }, {} };
    CHK_ERR(::bind(fd, reinterpret_cast<const sockaddr*>(&listen_addr),sizeof(sockaddr_in)) == 0);
    const int s_backlog = 0;
    CHK_ERR(::listen(fd, s_backlog) == 0);
    while(true) {
      sockaddr addr;
      socklen_t addr_len=sizeof(addr);
      auto conn = ::accept(fd, &addr, &addr_len);
      LOG("client connected at port " << port); 
      CHK_ERR(conn > 0);
   }

}


int main() {
 pthread_t h1,h2,h3,h4,h5;
 int exit_code;
 int port1=31017, port2=31224, port3=25249, port4=31019, port5=31013;
 pthread_create(&h1, NULL, start_server, &port1);
 pthread_create(&h2, NULL, start_server, &port2);
 pthread_create(&h3, NULL, start_server, &port3);
 pthread_create(&h4, NULL, start_server, &port4);
 pthread_create(&h5, NULL, start_server, &port5);

 pthread_join(h1, (void **) &exit_code); 
 pthread_join(h2, (void **) &exit_code);
 pthread_join(h3, (void **) &exit_code);
 pthread_join(h4, (void **) &exit_code);
 pthread_join(h5, (void **) &exit_code); 

}
