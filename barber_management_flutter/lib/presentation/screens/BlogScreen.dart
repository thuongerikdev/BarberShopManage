import 'package:flutter/material.dart';

class BlogScreen extends StatefulWidget {
  @override
  _BlogScreenState createState() => _BlogScreenState();
}

class _BlogScreenState extends State<BlogScreen> {
  final List<Map<String, String>> blogs = [
    {
      'title': 'Top 10 Kiểu Tóc Đẹp 2023',
      'summary': 'Khám phá những kiểu tóc hot nhất năm nay.',
      'image': 'https://via.placeholder.com/150',
    },
    {
      'title': 'Cách Chăm Sóc Tóc Tại Nhà',
      'summary': 'Bí quyết giữ tóc khỏe mạnh không cần salon.',
      'image': 'https://via.placeholder.com/150',
    },
    {
      'title': 'Xu Hướng Nhuộm Tóc Mới',
      'summary': 'Những màu tóc đang dẫn đầu xu hướng.',
      'image': 'https://via.placeholder.com/150',
    },
  ];

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);

  Future<void> _refreshData() async {
    // Giả lập làm mới dữ liệu
    await Future.delayed(Duration(seconds: 1)); // Giả lập thời gian load
    setState(() {
      // Cập nhật giao diện hoặc gọi API nếu có
    });
    _showSnackBar(context, "Dữ liệu blog đã được làm mới");
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      body: RefreshIndicator(
        onRefresh: _refreshData,
        color: accentColor,
        child: SingleChildScrollView(
          physics: BouncingScrollPhysics(),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Padding(
                padding: const EdgeInsets.fromLTRB(16, 20, 16, 12),
                child: Text(
                  "Danh Sách Blog",
                  style: TextStyle(
                    fontSize: 24,
                    fontWeight: FontWeight.w700,
                    color: textColor,
                    fontFamily: 'Poppins',
                    letterSpacing: 0.8,
                  ),
                ),
              ),
              ListView.builder(
                shrinkWrap: true,
                physics: NeverScrollableScrollPhysics(),
                itemCount: blogs.length,
                itemBuilder: (context, index) {
                  return Card(
                    margin: EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                    elevation: 8,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
                    color: primaryColor.withOpacity(0.9),
                    child: Padding(
                      padding: const EdgeInsets.all(16),
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          ClipRRect(
                            borderRadius: BorderRadius.circular(12),
                            child: Image.network(
                              blogs[index]['image']!,
                              width: 60,
                              height: 60,
                              fit: BoxFit.cover,
                              errorBuilder: (context, error, stackTrace) => Icon(Icons.broken_image, color: Colors.redAccent),
                            ),
                          ),
                          SizedBox(width: 16), // Khoảng cách giữa ảnh và chữ
                          Expanded(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  blogs[index]['title']!,
                                  style: TextStyle(
                                    fontSize: 18,
                                    fontWeight: FontWeight.w600,
                                    color: textColor,
                                    fontFamily: 'Poppins',
                                  ),
                                ),
                                SizedBox(height: 8),
                                Text(
                                  blogs[index]['summary']!,
                                  style: TextStyle(
                                    fontSize: 14,
                                    color: Colors.grey[400],
                                    fontFamily: 'Poppins',
                                  ),
                                  maxLines: 2,
                                  overflow: TextOverflow.ellipsis,
                                ),
                              ],
                            ),
                          ),
                          SizedBox(width: 10),
                          Icon(
                            Icons.arrow_forward_ios,
                            size: 22,
                            color: accentColor,
                          ),
                        ],
                      ),
                    ),
                  );
                },
              ),
              SizedBox(height: 40),
            ],
          ),
        ),
      ),
    );
  }

  void _showSnackBar(BuildContext context, String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(
          "Đã chọn $message",
          style: TextStyle(fontFamily: 'Poppins', fontWeight: FontWeight.w500, color: Colors.white),
        ),
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        backgroundColor: primaryColor,
        duration: Duration(seconds: 2),
      ),
    );
  }
}