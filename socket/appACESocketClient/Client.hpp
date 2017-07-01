#pragma once

#include "ace/SOCK_Connector.h"
#include "ace/INET_Addr.h"
#include "ace/Log_Msg.h"
#include "ace/OS.h"
#include <string.h>

//#include "ace/Time_Value.h"
//#include   <ace/ACE.h>
//#include   <ace/SOCK_Acceptor.h>
//#include   "ace/stream.h"
#define SIZE_BUF 128
#define NO_ITERATIONS 50
# pragma   comment (lib,   "C:\\ACE\\ACE-6.4.3\\ACE_wrappers\\lib\\ACE.lib")
class Client {
public:
	Client(char *hostname, int port) :remote_addr_(port, hostname)
	{
		data_buf_ = "Hello from Client";
	}

	int connect_to_server()
	{
		ACE_Time_Value timeout(30);
		ACE_DEBUG((LM_DEBUG, "(%P|%t) Starting connect to %s:%d\n",
			remote_addr_.get_host_name(), remote_addr_.get_port_number()));
		if (connector_.connect(client_stream_, remote_addr_, &timeout, remote_addr_) == -1)
			ACE_ERROR_RETURN((LM_ERROR, "(%P|%t) %p\n", "connection failed"), -1);
		else
			ACE_DEBUG((LM_DEBUG, "(%P|%t) connected to %s\n",
				remote_addr_.get_host_name()));
		return 0;
	}

	int send_to_server()
	{
		size_t len = std::strlen(data_buf_);// ACE_OS::strlen(data_buf_);
		ACE_Time_Value timeout(30);
		for (int i = 0; i<NO_ITERATIONS; i++) {
			if (client_stream_.send_n(data_buf_, 
				 len+ 1, 
				0,&timeout,NULL) == -1) {
			//if (client_stream_.send_n(data_buf_, 1024 + 1, 0, &timeout) == -1) {
				ACE_ERROR_RETURN((LM_ERROR, "(%P|%t) %p\n", "send_n"), 0);
				break;
			}
			else
			{
				ACE_DEBUG((LM_DEBUG, "sent\n"));
			}
		}
		close();
		return 0;
	}

	int close()
	{
		if (client_stream_.close() == -1)
			ACE_ERROR_RETURN((LM_ERROR, "(%P|%t) %p\n", "close"), -1);
		else
			return 0;
	}

private:
	ACE_SOCK_Stream client_stream_;
	ACE_INET_Addr remote_addr_;
	ACE_SOCK_Connector connector_;
	char *data_buf_;
};
