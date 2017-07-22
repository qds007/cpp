#include <pthread.h>
#include <stdio.h>
#include <netdb.h>
#include <stdlib.h>
#include <unistd.h>



void* func(void* sock)
{

  int fd = *(int*) sock;
  int sentData, recvData;
  write(fd,&sentData,sizeof(int));
  read(fd, &recvData,sizeof(int));
}

#define MAX_SERVER 16
pthread_t handles[MAX_SERVER];

int main()
{

   int sock=socket(AF_INET,SOCK_STREAM,0);
   struct hostent *host_info;
   struct sockaddr_in server;
   
   bool atLeastOne;
   for (int i=0;i<MAX_SERVER; i++)
      {
       if( connect(sock, (struct sockaddr*) &server, sizeof server)<0) {
         printf("cannot connect to server %i\n",i);
         continue;
       }
       else
         atLeastOne=true;

       pthread_create(&handles[i],NULL,func,&sock);
      }

      if(!atLeastOne) exit(1);

      for (int i=0;i<MAX_SERVER; i++)
      pthread_join(handles[i],NULL);

   printf("client running \n");
}
