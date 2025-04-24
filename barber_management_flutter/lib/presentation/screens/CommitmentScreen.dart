import 'package:flutter/material.dart';

class CommitmentScreen extends StatelessWidget {
  const CommitmentScreen({super.key});

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const shadowColor = Color(0xFF3E2723);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        title: Text(
          'Cam kết',
          style: TextStyle(
            color: textColor,
            fontFamily: 'Poppins',
            fontWeight: FontWeight.bold,
          ),
        ),
        backgroundColor: primaryColor,
        elevation: 0,
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: SingleChildScrollView(
        physics: BouncingScrollPhysics(),
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Title: CAM KẾT 30SHINE CARE
              Text(
                'CAM KẾT 30SHINE CARE',
                style: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 16),

              // Image placeholder (since the image is not provided, using a placeholder)
              Container(
                height: 200,
                width: double.infinity,
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(15),
                  color: shadowColor.withOpacity(0.3),
                ),
                child: Center(
                  child: Text(
                    'Hình ảnh minh họa',
                    style: TextStyle(
                      color: textColor.withOpacity(0.5),
                      fontSize: 16,
                      fontFamily: 'Poppins',
                    ),
                  ),
                ),
              ),
              SizedBox(height: 16),

              // Subtitle: VÌ 30SHINE TIN CHẤT LƯỢNG PHỤC VỤ LÀ HÀNG ĐẦU
              Text(
                'VÌ 30SHINE TIN CHẤT LƯỢNG PHỤC VỤ LÀ HÀNG ĐẦU',
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 8),

              // Description: Áp dụng tại salon bất kỳ toàn hệ thống 30Shine
              Text(
                'Áp dụng tại salon bất kỳ toàn hệ thống 30Shine',
                style: TextStyle(
                  fontSize: 14,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 16),

              // Section: 7 ngày chỉnh sửa tóc MIỄN PHÍ
              Text(
                '7 ngày chỉnh sửa tóc MIỄN PHÍ',
                style: TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 8),
              Text(
                'Nếu anh chưa hài lòng về kiểu tóc sau khi vừa ký lý do gì, 30Shine sẽ hỗ trợ anh sửa lại mái tóc đó hoàn toàn MIỄN PHÍ trong vòng 7 ngày. Anh đặt lịch bình thường và bảo sửa tóc với lễ tân.',
                style: TextStyle(
                  fontSize: 14,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 16),

              // Section: 30 ngày đổi/trả hàng MIỄN PHÍ
              Text(
                '30 ngày đổi/trả hàng MIỄN PHÍ',
                style: TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 8),
              Text(
                'Tất cả các sản phẩm mua tại 30Shine anh có thể đổi hoặc trả lại hoàn toàn MIỄN PHÍ (hoàn lại 100% số tiền) trong vòng 30 ngày kể từ thời điểm mua hàng, ngay cả khi sản phẩm đó đã qua sử dụng.',
                style: TextStyle(
                  fontSize: 14,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 16),

              // Section: Cam kết: Hoàn lại 100% tiền.
              Text(
                'Cam kết: Hoàn lại 100% tiền.',
                style: TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 16),

              // Section: 7 ngày bảo hành Uốn/Nhuộm
              Text(
                '7 ngày bảo hành Uốn/Nhuộm',
                style: TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 8),
              Text(
                'Mái tóc sau khi uốn nhuộm có thể không đúng ý anh sau khi về nhà, 30Shine sẽ hỗ trợ anh chỉnh sửa lại mái tóc đó miễn phí trong vòng 7 ngày. Anh đặt lịch bình thường và chọn dịch vụ bảo hành tóc với lễ tân.',
                style: TextStyle(
                  fontSize: 14,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 16),

              // Section: Có chính sách giả đặc biệt nếu cho là lựa chọn sai
              Text(
                'Có chính sách giả đặc biệt nếu cho là lựa chọn sai',
                style: TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 8),
              Text(
                '30Shine cam kết phục vụ anh đúng giờ đặt lịch. Nếu anh phải chờ lâu hơn 20 phút so với giờ đặt lịch, 30Shine sẽ có chính sách giả đặc biệt dành riêng cho anh.\n\nÁp dụng: Khi anh đến đúng giờ đặt lịch. Thời gian chờ được tính từ lúc checkin tới lúc bắt đầu gội. Trừ trường hợp salon xảy ra sự cố bất khả kháng như mất điện, nước.',
                style: TextStyle(
                  fontSize: 14,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 16),

              // Section: CHỊ TRÔNG 2 GIỜ BÔN EM SẼ HỖ TRỢ ANH NGAY
              Text(
                'CHỊ TRÔNG 2 GIỜ BÔN EM SẼ HỖ TRỢ ANH NGAY',
                style: TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 8),
              Text(
                'Nếu các anh chưa được báo giá, thông tin dịch vụ rõ ràng, minh bạch trước khi sử dụng chúng em xin phép hoàn trả ngay 100% giá trị anh đã thanh toán\n\nKiểu tóc mới chưa quen mắt hay anh căng chỉnh lại, thay đổi bất cứ điều gì nếu chưa đến kịp Salon anh yên tâm, chúng em có đội ngũ đến tận nhà chỉnh lại ngay\n\nNgại ra, bất cứ điều gì trong trải nghiệm này thiệt hại đến anh như thời gian, sức khoẻ, công việc, salon sẽ hỗ trợ anh ngay\n\nChị trông 2 giờ bôn em sẽ hỗ trợ anh ngay\n\n(100% Sản phẩm sử dụng tại 30Shine đều có nguồn gốc rõ ràng)',
                style: TextStyle(
                  fontSize: 14,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 20),
            ],
          ),
        ),
      ),
    );
  }
}