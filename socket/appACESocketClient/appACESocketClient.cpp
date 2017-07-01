#include "stdafx.h"
#include "Client.hpp"
#include "ace/Log_Msg.h"
#include "ace/OS.h"
# pragma   comment (lib,   "C:\\ACE\\ACE-6.4.3\\ACE_wrappers\\lib\\ACE.lib")

int main(int argc, char *argv[])
{
	//if (argc<3) {
	//	ACE_DEBUG((LM_DEBUG, "Usage %s <hostname> <port_number>\n", argv[0]));
	//	ACE_OS::exit(1);
	//}
	Client client("HPF", 4502);
	//Client client(argv[1], 4502);// ACE_OS::atoi(argv[2]));
	client.connect_to_server();
	client.send_to_server();
	getchar();
	client.close();
	return 0;
}


