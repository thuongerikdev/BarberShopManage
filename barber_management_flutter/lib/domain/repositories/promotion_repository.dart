import 'package:barbermanagemobile/domain/entities/vip/promotion.dart';

abstract class PromotionRepository {
  Future<List<Promotion>> getAllPromoByCustomer(int customerId);
  Future<void> createCustomerPromotion(int customerId, int promoId, String status);
}