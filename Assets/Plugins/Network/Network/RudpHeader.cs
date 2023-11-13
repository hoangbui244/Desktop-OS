using System;
using System.Collections.Generic;
using System.Text;
namespace NGE.Network
{
	//�Ǳ�׼��RUDPЭ��ͷ
	//----------------------------------------------------------------
	//| 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 |          16bit               |
	//----------------------------------------------------------------
	//|          Control Bits         |                              |
	//|SYN|ACK|EAK|RST|NUL|CHK|TCS| 0 |       Packet Length          |
	//----------------------------------------------------------------
	//|           Sequence number     |       Ack number             |
	//----------------------------------------------------------------
	//|           Header Checksum     |       Data Checksum          |
	//----------------------------------------------------------------
	////////////////////////////////////////////////////////////////////////
	//        Control Bits
	//SYN: SYN λ��ʾ��ǰΪͬ���� 
	//ACK: ACK λ��ʾЭ��ͷ��Ч�ĳ�����š� 
	//EACK:EACKλ��ʾ��ǰΪ��չ�����ֶΡ� 
	//RST: RST λ��ʾ�����ݰ�Ϊ��λ�ֶΡ� 
	//NUL: NUL λ��ʾ�����ݰ�Ϊ���ֶΡ�. 
	//CHK: CHK λ��ʾ������ֶ��Ƿ����Э��ͷ��Э��ͷ�����壨���ݣ��ļ���͡� 
	//TCS: TCS λ��ʾ�����ݰ��Ǵ�������״̬�ֶΡ� 
	//0:   ��ʾ���ֶε�ֵ��������Ϊ0��
	internal struct RudpHeader
	{
	}
}