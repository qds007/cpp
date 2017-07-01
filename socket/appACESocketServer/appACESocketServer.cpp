#include "stdafx.h"
#include "Server.hpp"
#include "ace/Log_Msg.h"
#include "ace/OS.h"

int main(int argc, char *argv[])
{
	char* p = new char[10];
	*p = 'a';
	*(p + 2) = 'g';
	auto i = *(p + 2);
	auto j = *(p + 1);
	if (argc<2) {
		ACE_ERROR((LM_ERROR, "Usage %s <port_num>", argv[0]));
		//ACE_OS::exit(1);
	}
	//Server server(ACE_OS::atoi(argv[1]));
	Server server(4502);
	server.accept_connections();

	getchar();
    return 0;
}

